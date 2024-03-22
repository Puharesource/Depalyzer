namespace Depalyzer.Cli.Process;

using System.Diagnostics;
using System.Text;

public class CommandProcess : IDisposable
{
    private readonly Process handle;
    private readonly StringBuilder outputBuilder;

    public CommandProcess(string workingDirectory, string fileName, string arguments)
    {
        this.handle = new();
        this.outputBuilder = new();

        var startInfo = this.handle.StartInfo;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.CreateNoWindow = true;
        startInfo.WorkingDirectory = workingDirectory;
        startInfo.FileName = fileName;
        startInfo.Arguments = arguments;

        this.handle.EnableRaisingEvents = true;
        this.handle.OutputDataReceived += delegate(object _, DataReceivedEventArgs eventArgs) { this.outputBuilder.AppendLine(eventArgs.Data); };
        this.handle.ErrorDataReceived += delegate(object _, DataReceivedEventArgs eventArgs) { this.outputBuilder.AppendLine(eventArgs.Data); };
    }

    public async Task<string> Run()
    {
        this.handle.Start();

        this.handle.BeginOutputReadLine();
        this.handle.BeginErrorReadLine();
        await this.handle.WaitForExitAsync();
        this.handle.CancelOutputRead();
        this.handle.CancelErrorRead();

        this.handle.Close();

        return this.outputBuilder.ToString();
    }

    public void Dispose()
    {
        this.handle.Dispose();
        GC.SuppressFinalize(this);
    }
}