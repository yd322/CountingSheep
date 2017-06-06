using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public AudioClip[] songs;

	private AudioSource musicSource;

	void Awake()
	{
		musicSource = GetComponent<AudioSource>();

		PlaySong();
	}

	private void PlaySong()
	{
		AudioClip currentSong = PlayRandomSoundFromSource(musicSource, songs);

		if (currentSong != null)
		{
			Invoke("PlaySong", currentSong.length);
		}
		else
		{
			Debug.Log("Null audio clip encountered in SoundManager.PlaySong(). Halting music.");
		}
	}

	public static AudioClip PlayRandomSoundFromSource(AudioSource source, params AudioClip[] clips)
	{
		if (source != null && clips.Length > 0)
		{
			int choice = Random.Range(0, clips.Length);

			if (clips[choice] != null)
			{
				source.PlayOneShot(clips[choice]);

				return clips[choice];
			}
		}

		return null;
	}
}
