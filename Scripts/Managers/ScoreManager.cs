using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	//our score
	int m_score = 0;

	// the number of lines we need to get to the next level
	int m_lines;

	// our current level
	public int m_level = 1;

	// base number of lines needed to clear a level
	public int m_linesPerLevel = 5;

	public Text m_linesText;
	public Text m_levelText;
	public Text m_scoreText;
    public Text m_highScoreText;

	// minimum number of lines we can clear if we do indeed clear any lines
	const int m_minLines = 1;

	// maximum number of lines we can clear if we do indeed clear any lines
	const int m_maxLines = 4;

	//
	public bool didLevelUp = false; 

	public ParticlePlayer m_levelUpFx;

	// update the user interface
	void UpdateUIText()
	{
		if (m_linesText)
			m_linesText.text = m_lines.ToString();

		if (m_levelText)
			m_levelText.text = m_level.ToString();

		if (m_scoreText)
			m_scoreText.text = PadZero(m_score,5);

        if (m_highScoreText)
            m_highScoreText.text = PadZero(PlayerPrefs.GetInt("HighScore", 0), 5);
	}

	// handle scoring
	public void ScoreLines(int lines, int combo)
	{
		// flag to GameController that we leveled up
		didLevelUp = false;

		// clamp this between 1 and 4 lines
		lines = Mathf.Clamp(lines,m_minLines,m_maxLines);

        int score = 0;

        switch (lines)
        {
            case 1:
                score = 40 * m_level;
                break;
            case 2:
                score = 100 * m_level;
                break;
            case 3:
                score = 300 * m_level;
                break;
            case 4:
                score = 1200 * m_level;
                break;
        }

        if (combo > 0 && combo < 5)
        {
            score *= 2;
        }
        else if (combo > 4 && combo < 10)
        {
            score *= 3;
        }
        else if (combo > 9)
        {
            score *= 5;
        }

        m_score += score;

        //Set the highscore
        if(PlayerPrefs.GetInt("HighScore", 0) < m_score)
        {
            PlayerPrefs.SetInt("HighScore", m_score);
        }

        // reduce our current number of lines needed for the next level
        m_lines -= lines;

		// if we finished our lines, then level up
		if (m_lines <= 0)
		{
			LevelUp();
		}

		// update the user interface
		UpdateUIText();
	}

	// start our level and lines -- in the future we might start at a different level than 1 for increased difficulty
	public void Reset()
	{
		m_level = 1;
		m_lines = m_linesPerLevel * m_level;
		UpdateUIText();
	}

	// increments our level
	public void LevelUp()
	{
		m_level++;
		m_lines = m_linesPerLevel* m_level;
		didLevelUp = true;

		if (m_levelUpFx)
		{
			m_levelUpFx.Play();

		}
	}

	void Start () 
	{
		Reset();
	}

	// returns a string padded to a certain number of places
	string PadZero(int n,int padDigits)
	{
		string nStr = n.ToString();

		while (nStr.Length < padDigits)
		{
			nStr = "0" + nStr;
		}
		return nStr;
	}



}
