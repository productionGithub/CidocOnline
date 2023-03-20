#define TRACE_OFF
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Scenes.Board.Card.Cards;

namespace StarterCore.Core.Scenes.Board.Challenge
{
    /// <summary>
    ///Parse all properties and process each of __Answer fields
    ///Evaluate each property's answer, result is stored in global 'isCorrect' flag
    ///If at least one expected answer is false, we stop and return 'isCorrect' with the value 'false'
    ///If none of property has been evaluated to false, the 'isCorrect' flag is returned with initial value: 'true'
    /// </summary>
    /// <param name="expectedResults"></param>
    /// <param name="playerResults"></param>
    /// <returns></returns>
    ///

    public class ChallengeEvaluator : MonoBehaviour
    {
        [Inject] EntityDeckService _entityDeckService;
        [Inject] PropertyDeckService _propertyDeckService;

        private ParserTool parser;//String parsing utility class

        private List<string> props;

        private bool _isInitDone = false;

        public void Init()
        {
            if (_isInitDone == false)
            {
                props = new List<string>() {
                    "ELeftAnswer",
                    "EMiddleAnswer",
                    "ERightAnswer",
                    "PLeftAnswer",
                    "PRightAnswer"
                };

                parser = new ParserTool();

                _isInitDone = true;
            }
        }
        
        public bool CheckAnswers(ChallengeData expectedResults, ChallengeData playerResults)
        {
            // We assume the proposition is correct.
            bool isCorrect = true; //Global challenge result returned: true = OK, false = wrong answer

            foreach (var prop in props)
            {
                switch (prop)
                {
                    case "ELeftAnswer":
                        string leftEntityPlayerAnswer = playerResults.ELeftAnswer;
                        string leftEntityPossibleAnswers = expectedResults.ELeftAnswer;

                        if (leftEntityPossibleAnswers != string.Empty)
                        {
                            //Remove potential white spaces in expected answers string
                            string beautifulPossibleAnswers = parser.TrimWhiteSpace(leftEntityPossibleAnswers);
                            //if expected result is '*' then no need to evaluate, any answer matches.
                            if (beautifulPossibleAnswers != "*")
                            {
                                //If this answser is false, the whole answer is wrong, isCorrect will shift to false
                                isCorrect = EvaluateEntityAnswer(leftEntityPlayerAnswer, beautifulPossibleAnswers);
                            }
                        }
                        break;

                    case "EMiddleAnswer":
                        string middlePlayerAnswer = playerResults.EMiddleAnswer;
                        string middlePossibleAnswers = expectedResults.EMiddleAnswer;

                        if (middlePossibleAnswers != string.Empty)
                        {
                            //Remove potential white spaces in expected answers string
                            string beautifulPossibleAnswers = parser.TrimWhiteSpace(middlePossibleAnswers);

                            //if expected result is '*' then no need to evaluate, any answer matches.
                            if (!parser.isWildCard(beautifulPossibleAnswers))
                            {
                                //If this answser is false, the whole answer is wrong, isCorrect will shift to false
                                isCorrect = EvaluateEntityAnswer(middlePlayerAnswer, beautifulPossibleAnswers);
                            }
                        }
                        break;

                    case "ERightAnswer":
                        string rightPlayerAnswer = playerResults.ERightAnswer;
                        string rightPossibleAnswers = expectedResults.ERightAnswer;

                        if (rightPossibleAnswers != string.Empty)
                        {
                            //Remove potential white spaces in expected answers string
                            string beautifulPossibleAnswers = parser.TrimWhiteSpace(rightPossibleAnswers);
                            //if expected result is '*' then no need to evaluate, any answer matches.
                            if (!parser.isWildCard(beautifulPossibleAnswers))
                            {
                                //If this answser is false, the whole answer is wrong, isCorrect will shift to false
                                isCorrect = EvaluateEntityAnswer(rightPlayerAnswer, beautifulPossibleAnswers);
                            }
                        }
                        break;

                    case "PLeftAnswer":
                        string pLeftPlayerAnswer = playerResults.PLeftAnswer;
                        string pLeftPossibleAnswers = expectedResults.PLeftAnswer;

                        if (pLeftPossibleAnswers != string.Empty)
                        {
                            //Remove potential white spaces in expected answers string
                            string beautifulPossibleAnswers = parser.TrimWhiteSpace(pLeftPossibleAnswers);
                            //if expected result is '*' then no need to evaluate, any answer matches.
                            if (!parser.isWildCard(beautifulPossibleAnswers))
                            {
                                //If this answser is false, the whole answer is wrong, isCorrect will shift to false
                                isCorrect = EvaluatePropertyAnswer(pLeftPlayerAnswer, beautifulPossibleAnswers);
                            }
                        }
                        break;

                    case "PRightAnswer":
                        string pRightPlayerAnswer = playerResults.PRightAnswer;
                        string pRightPossibleAnswers = expectedResults.PRightAnswer;

                        if (pRightPossibleAnswers != string.Empty)
                        {
                            //Remove potential white spaces in expected answers string
                            string beautifulPossibleAnswers = parser.TrimWhiteSpace(pRightPossibleAnswers);
                            //if expected result is '*' then no need to evaluate, any answer matches.
                            if (!parser.isWildCard(beautifulPossibleAnswers))
                            {
                                //If this answser is false, the whole answer is wrong, isCorrect will shift to false
                                isCorrect = EvaluatePropertyAnswer(pRightPlayerAnswer, beautifulPossibleAnswers);
                            }
                        }
                        break;
                }
                if (isCorrect == false) break;//If one is false, the whole challenge is false
            }
            return isCorrect;
        }



        private bool EvaluateEntityAnswer(string answer, string possibleAnswers)
        {
            bool eval = false;

            char selector = possibleAnswers.ToString()[0];

            switch (selector)
            {
                case '-':
                    //Remove the selector in possible answers
                    possibleAnswers = parser.TrimSelector(possibleAnswers);
                    string[] answersList = possibleAnswers.Split(',');
                    foreach (string possible in answersList)
                    {
                        if (answer == possible)
                        {
                            eval = true;
                            break;
                        }
                        else eval = false;
                        //V3 -> Check branch with card colors / expected card colors
                        //If !possible.colors.Contains(_entityDeckService[answer.colors]) => Wrong branch (just the idea)
                        //If !WrongBranch { If( !answer == possible) => Wrong class / Entity)
                    }

                    break;

                case ':':
                    //Remove the selector in possible answers
                    possibleAnswers = parser.TrimSelector(possibleAnswers);
                    string[] colorList = possibleAnswers.Split(',');

                    EntityCard card = _entityDeckService.EntityCards.Single(c => c.id == answer); ;

                    //Create a list of color32 from expected results
                    List<Color32> colors = new List<Color32>();

                    foreach (string color in colorList)
                    {
                        colors.Add(_entityDeckService.ColorsDictionary[color]);
                    }

                    switch (card.colors.Count)
                    {
                        case 1://one color to test
                            if (colors.Contains(card.colors[0])) eval = true;
                            break;
                        case 2://Two colors to test
                            if (colors.Contains(card.colors[0]) || colors.Contains(card.colors[1])) eval = true;
                            break;
                        default:
                            return false;
                    }
                    break;

                default:
                    break;
            }
            return eval;
        }


        private bool EvaluatePropertyAnswer(string answer, string possibleAnswers)
        {
            bool eval = false;

            char selector = possibleAnswers.ToString()[0];

            switch (selector)
            {
                case '-':
                    //Remove the selector in possible answers
                    possibleAnswers = parser.TrimSelector(possibleAnswers);
                    string[] answersList = possibleAnswers.Split(',');
                    foreach (string possible in answersList)
                    {
                        if (answer == possible)
                        {
                            eval = true;
                            break;
                        }
                        else eval = false; // => Wrong property !
                    }

                    break;

                case '[':
                    //Process [;]
                    //Remove selector ([...])
                    possibleAnswers = parser.TrimSelector(possibleAnswers);//[:Yellow;:Brown]

                    string[] allColorsList = possibleAnswers.Split(';');//:Yellow;:Brown

                    //For readability: store the 2 lists in 2 variables
                    string[] domainColorArray = allColorsList[0].Split(',');//:Yellow, ...
                    string[] rangeColorArray = allColorsList[1].Split(','); //:Brown, ...

                    //Remove the ':' on each first element of the list
                    domainColorArray[0] = domainColorArray[0].TrimStart(':');//Yellow, ...
                    rangeColorArray[0] = rangeColorArray[0].TrimStart(':');//Brown, ...

                    //Convert string[] to List to use Linq
                    List<string> domainColorList = domainColorArray.ToList();
                    List<string> rangeColorList = rangeColorArray.ToList();

                    //Get reference to the card submitted
                    PropertyCard card = _propertyDeckService.PropertyCards.Single(c => c.id == answer);

                    //Test domain Colors
                    bool evalDomain = false;
                    var resultDomainList = card.domainColors.Where(a => domainColorList.Any(b => b.Contains(a)));
                    if (resultDomainList.Count() > 0) evalDomain = true; else evalDomain = false;

                    //Test range Colors
                    bool evalRange = false;
                    var resultRangeList = card.domainColors.Where(a => domainColorList.Any(b => b.Contains(a)));
                    if (resultRangeList.Count() > 0) evalRange = true; else evalRange = false;

                    if (evalDomain == true && evalRange == true) eval = true;
                    break;

                default:
                    break;
            }
            return eval;
        }
    }
}
