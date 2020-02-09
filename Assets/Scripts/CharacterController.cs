using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Animator animator;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public Vector3 facingDirection = Vector3.zero;

    [SerializeField] List<AudioSource> audioSources;

    bool whichAudioSource;

    [SerializeField] List<AudioClip> selectedSounds;
    [SerializeField] List<AudioClip> acknowledgeSounds;
    [SerializeField] List<AudioClip> annoyedSounds;

    public bool annoyed = false;

    Stack<PathFind.Point> currentPath = new Stack<PathFind.Point>();

    Vector3 mouseClickPos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = this.transform.position;
        mouseClickPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentPath.Count == 1)
            //target = new Vector3(mouseClickPos.x, mouseClickPos.y, transform.position.z);
        if (currentPath.Count > 1 &&
            Vector3.Distance(target, transform.position) <= 0.15f)
        {
            currentPath.Pop();
            facingDirection = (target - this.transform.position).normalized;
            target = new Vector3(currentPath.Peek().x, currentPath.Peek().y, transform.position.z);
        }
        else if (Vector3.Distance(target, transform.position) < 0.15f)
        {
            animator.SetBool("Moving", false);
        }

        if (currentPath.Count > 1)
            facingDirection = (target - this.transform.position).normalized;
        if (facingDirection.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;
        animator.SetFloat("XDir", facingDirection.x);
        animator.SetFloat("YDir", facingDirection.y);

        transform.position = Vector3.MoveTowards(transform.position, target, 1.5f * Time.deltaTime);
    }

    //Public functions get called by UnitManager class

    public void OnSelect()
    {
        GetComponent<SpriteOutline>().enabled = true;
        if (annoyed)
        {
            annoyed = false;
            PlaySound(annoyedSounds);
        }
        else
        {
            PlaySound(selectedSounds);
        }
    }

    public void OnDeselect()
    {
        GetComponent<SpriteOutline>().enabled = false;
    }

    public void StartMove(Vector3 tilePos, Vector3 mousePos)
    {
        currentPath.Clear();
        currentPath = Navigator.GetPath((int)transform.position.x, (int)-transform.position.y,
                                        (int)tilePos.x, (int)-tilePos.y);
        mouseClickPos = mousePos;
        if (currentPath.Count > 0)
        {
            target = new Vector3(currentPath.Peek().x, currentPath.Peek().y, this.transform.position.z);
            animator.SetBool("Moving", true);
        }
        PlaySound(acknowledgeSounds);
    }

    // Crossfades one audio source with the other for smooth clip transitions.
    void PlaySound(List<AudioClip> soundChoices)
    {
        StartCoroutine(StartFade(audioSources[Convert.ToInt32(whichAudioSource)], 0.16f, 0.0f));
        whichAudioSource = !whichAudioSource;
        audioSources[Convert.ToInt32(whichAudioSource)].volume = 1.0f;
        audioSources[Convert.ToInt32(whichAudioSource)].PlayOneShot(soundChoices[UnityEngine.Random.Range(0, soundChoices.Count)]);
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
