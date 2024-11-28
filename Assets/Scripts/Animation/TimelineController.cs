using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    private PlayableDirector playableDirector;

    private void Start()
    {
        playableDirector = FindObjectOfType<PlayableDirector>();
        if (playableDirector != null)
        {
            // playableDirector.Play();
            playableDirector.stopped += OnTimelineStopped;
        }
        else
        {
            Debug.LogError("PlayableDirector가 씬에 존재하지 않습니다.");
        }
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
