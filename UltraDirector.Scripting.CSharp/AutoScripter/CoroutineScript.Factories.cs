using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using UltraDirector.Scripting.CSharp.Utils;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace UltraDirector.Scripting.CSharp.AutoScripter;

public sealed partial class CoroutineScript
{
    private static readonly ScriptOptions ScriptOptions =
        ScriptOptions.Default
                     .AddReferences(from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                    where !assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location)
                                    select assembly);

    // TODO: look into compiling as Regular instead of script so i dont get the overhead of the state machine
    //       and can get an easier to analyse IL
    // TODO: support helpers to define hooks. perhaps methods annotated with [Hook(Something)]? And replace with calls 
    public static async Task<CoroutineScript> CreateAsync(string code, CancellationToken token = default)
    { // note that basically everything in c# compilations is immutable so remember to store returned values
        var tree = CSharpSyntaxTree.ParseText(code,
            new CSharpParseOptions(kind: SourceCodeKind.Script));
        var root = tree.GetCompilationUnitRoot(token);
        var topLevelStatements = root.Members
                                     .Select(static m => m switch
                                      {
                                          GlobalStatementSyntax gss => gss.Statement,
                                          FieldDeclarationSyntax fd => LocalDeclarationStatement(
                                              (VariableDeclarationSyntax)fd.ChildNodes()
                                                                           .First(static sn =>
                                                                                sn is VariableDeclarationSyntax)),
                                          _ => null
                                      })
                                     .WhereNotNull()
                                     .ToArray();
        var iEnumeratorIdentifier = IdentifierName(nameof(IEnumerator));

        var privateModifier = Token(SyntaxKind.PrivateKeyword);
        var staticModifier = Token(SyntaxKind.StaticKeyword);
        var methodBody = Block(topLevelStatements);
        var wrappedMethodIdentifier = Identifier("___CreateScriptEnumerator");
        var wrappedMethod = MethodDeclaration(iEnumeratorIdentifier, wrappedMethodIdentifier)
                           .AddModifiers(privateModifier, staticModifier)
                           .WithBody(methodBody);

        var returnStatement =
            GlobalStatement(ReturnStatement(IdentifierName(wrappedMethodIdentifier)));
        var SystemCollectionsUsing = UsingDirective(IdentifierName("System.Collections"));
        var SystemUsing = UsingDirective(IdentifierName("System"));
        var newMembers = root.Members
                             .Where(m => m is not GlobalStatementSyntax)
                             .Append(wrappedMethod)
                             .Append(returnStatement);
        var newRoot = root.WithUsings(root.Usings.AddRange([SystemCollectionsUsing, SystemUsing]))
                          .WithMembers(new SyntaxList<MemberDeclarationSyntax>(newMembers))
                          .NormalizeWhitespace();
        var newTree = tree.WithRootAndOptions(newRoot, tree.Options);
        var t = await newTree.GetTextAsync(token);
        var factory = await CSharpScript.EvaluateAsync<Func<IEnumerator>>(t.ToString(), ScriptOptions, cancellationToken: token);
        return new CoroutineScript(factory);
    }

    public static async Task<CoroutineScript> CreateFromFileAsync(string scriptFilePath,
        CancellationToken token = default)
    {
        if (!File.Exists(scriptFilePath))
            throw new FileNotFoundException($"Script file not found: {scriptFilePath}");
        var scriptText = await File.ReadAllTextAsync(scriptFilePath, token);
        return await CreateAsync(scriptText, token);
    }
}