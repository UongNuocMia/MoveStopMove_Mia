using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
public class FloatingScoreUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Player player;
    private void Start()
    {
        player.OnKillEnemy += SetText;
        scoreText.text = "";
    }

    private void SetText(object sender, Player.OnKillEnemyEventArgs e)
    {
        Vector3 newPosition = scoreText.transform.position + new Vector3(0, 1.5f, 0);
        scoreText.text = e.score.ToString();
        scoreText.transform.DOMove(newPosition, 1.2f).OnComplete(OnComplete);
    }
    public void OnComplete()
    {
        scoreText.text = "";
        scoreText.transform.localPosition = Vector3.zero;
    }
}