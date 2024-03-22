namespace Depalyzer.Cli.Exceptions;

using Spectre.Console.Rendering;

public class RenderableException(IRenderable renderable) : Exception
{
    public IRenderable Renderable => renderable;
}