using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using Syntax = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ClassGeneratorUsingRoslyn
{
    public static class RosylnClassCreator
    {
        public static string GenerateClass()
        {
            var @consoleWriteLine = Syntax.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                                 Syntax.IdentifierName("Console"),
                                                                 name: Syntax.IdentifierName("WriteLine"));


            var @arguments = Syntax.ArgumentList(Syntax.SeparatedList(new[]{
              Syntax.Argument (
                  Syntax.LiteralExpression (
                      SyntaxKind.StringLiteralExpression,
                      Syntax.Literal (@"$""{this.Name} -> {this.counter}""", "${this.Name} -> {this.counter}")))
                    }));

            var @consoleWriteLineStatement = Syntax.ExpressionStatement(Syntax.InvocationExpression(@consoleWriteLine, @arguments));

            var @voidType = Syntax.ParseTypeName("void");
            var @stringType = Syntax.ParseTypeName("string");

            var @field = Syntax.FieldDeclaration(Syntax.VariableDeclaration(Syntax.ParseTypeName("int"), Syntax.SeparatedList(new[] { Syntax.VariableDeclarator(Syntax.Identifier("counter")) }))).AddModifiers(Syntax.Token(SyntaxKind.PrivateKeyword)).WithSemicolonToken(Syntax.Token(SyntaxKind.SemicolonToken));
            var @property = Syntax.PropertyDeclaration(stringType, "Name").AddAccessorListAccessors(Syntax.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                           .WithSemicolonToken(Syntax.Token(SyntaxKind.SemicolonToken)
                            )).AddModifiers(Syntax.Token(SyntaxKind.PublicKeyword));

            @property = @property.AddAccessorListAccessors(Syntax.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Syntax.Token(SyntaxKind.SemicolonToken)));

            var @printMethod = Syntax.MethodDeclaration(voidType, "Print").AddModifiers(Syntax.Token(SyntaxKind.PublicKeyword)).WithBody(Syntax.Block(consoleWriteLineStatement));
            

            
            List<ParameterSyntax> @parameterList = new List<ParameterSyntax>
            {
                 Syntax.Parameter(Syntax.Identifier("x")).WithType(Syntax.ParseTypeName("int"))
            };
            var @methodBody = Syntax.ParseStatement("counter += x;");
            var @incrementMethod = Syntax.MethodDeclaration(voidType, "Increment").AddModifiers(Syntax.Token(SyntaxKind.PublicKeyword)).WithBody(Syntax.Block(methodBody)).AddParameterListParameters(parameterList.ToArray());
            
            
            var @class = Syntax.ClassDeclaration("MyClass").WithMembers(Syntax.List(new MemberDeclarationSyntax[] { @property, @field, @incrementMethod, @printMethod  })).AddModifiers(Syntax.Token(SyntaxKind.PublicKeyword)).AddModifiers(Syntax.Token(SyntaxKind.SealedKeyword));

            var adhocWorkSpace = new AdhocWorkspace();
            adhocWorkSpace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format(@class, adhocWorkSpace);

            return formattedCode.ToFullString();
        }
    }
}
