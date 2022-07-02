using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
	public Text nameText;
	public Text dialogueText;

	private Animator animator;

	private Queue<string> sentences;
	private DialogTrigger triggerD;

	// Use this for initialization
	private void Start()
	{
		animator = GetComponent<Animator>();
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialog dialogue, DialogTrigger trigger)
	{
		triggerD = trigger;

		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	protected virtual void EndDialogue()
	{
		animator.SetBool("IsOpen", false);

		triggerD.OnEndDialog();
	}
}
