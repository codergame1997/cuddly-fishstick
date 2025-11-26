using UniRx;

public class PromptService
{
    public readonly Subject<string> OnPrompt = new Subject<string>();

    public void Show(string message) => OnPrompt.OnNext(message);
}