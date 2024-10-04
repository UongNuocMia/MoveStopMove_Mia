using UnityEngine.UI;

public class Lose : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        UIManager.Ins.OpenUI<MainMenu>();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        Close(0);
    }
    private void OnEnable()
    {
        score.text = GameManager.Ins.PlayerScore.ToString();
    }
}