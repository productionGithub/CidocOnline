<?php
    include("connect.php");
    header('Content-Type: application/json');

    if (!$connection->ping()) {
      printf ("Error: %s\n", $connection->error);
    }

    $userId = $_GET["UserId"];
    $scenarioName = $_GET["ScenarioName"];
    $chapterName = $_GET["ChapterName"];

    if($userId != null)
    {
        $historyId  = GetHistory($connection, $userId);
        echo "historyId : ".$historyId;
        echo "\n";
        $scenarioId = GetScenarioID($connection, $scenarioName);
        echo "scenarioId : ".$scenarioId;
        echo "\n";
        $chapterId = GetChapterID($connection, $scenarioId, $chapterName);
        echo "chapterId : ".$chapterId;
        echo "\n";
        $sessionId = GetSessionId($connection, $historyId, $scenarioId, $chapterId);
        echo "sessionId : ".$sessionId;
        echo "\n";
        $progressionId = GetProgression($connection, $sessionId, $chapterId);
        echo "sessionId : ".$progressionId;
        echo "\n";
    }

    function GetHistory($connection, $userId)
    {
        $sql = "SELECT * FROM ontomatchgame.History WHERE ontomatchgame.History.userId = $userId";
        $result = mysqli_query($connection, $sql);

        if($result)
        {
            if($result -> num_rows > 0)
            {
                while($row = mysqli_fetch_array($result))
                {
                    $historyId = $row['historyId'];
                }
            }
        }
        return $historyId;
    }

    function GetScenarioID($connection, $scenarioName)
    {
        $sql = "SELECT * FROM ontomatchgame.Scenario WHERE ontomatchgame.Scenario.scenarioName = '{$scenarioName}'";
        $result = mysqli_query($connection, $sql);

        if($result)
        {
            if($result -> num_rows > 0)
            {
                while($row = mysqli_fetch_array($result))
                {
                    $scenarioId = $row['scenarioId'];
                }
            }
        }
        return $scenarioId;
    }

    function GetChapterID($connection, $scenarId, $chapName)
    {
        // echo "GetChapterId";
        // echo "\n";
        // echo "scenarioID".$scenarId;
        // echo "\n";
        // echo "chapterName".$chapName;
        // echo "\n";

        $sql = " SELECT * FROM ontomatchgame.Chapter WHERE ontomatchgame.Chapter.scenarioId = $scenarId AND ontomatchgame.Chapter.chapterName = '{$chapName}' ";
        $result = mysqli_query($connection, $sql);

        if($result)
        {
            if($result -> num_rows > 0)
            {
                while($row = mysqli_fetch_array($result))
                {
                    $chapterId = $row['chapterId'];
                }
            }
        }
        return $chapterId;
    }

    function GetSessionID($connection, $histId, $scenarId, $chapId)
    {
        // echo "GetChapterId";
        // echo "\n";
        // echo "scenarioID".$scenarId;
        // echo "\n";
        // echo "chapterName".$chapName;
        // echo "\n";

        $sql = " SELECT * FROM ontomatchgame.Session WHERE ontomatchgame.Session.historyId = $histId AND ontomatchgame.Session.scenarioId = '{$scenarId}' AND ontomatchgame.Session.lastChapterId = '{$chapId}'";
        $result = mysqli_query($connection, $sql);

        if($result)
        {
            if($result -> num_rows > 0)
            {
                while($row = mysqli_fetch_array($result))
                {
                    $sessionId = $row['sessionId'];
                }
            }
        }
        return $sessionId;
    }

    function GetProgression($connection, $sessId, $chapId)
    {
        // echo "GetChapterId";
        // echo "\n";
        // echo "scenarioID".$scenarId;
        // echo "\n";
        // echo "chapterName".$chapName;
        // echo "\n";

        $sql = " SELECT * FROM ontomatchgame.Progression WHERE ontomatchgame.Progression.sessionId = $sessId AND ontomatchgame.Progression.chapterId = '{$chapId}' ";
        $result = mysqli_query($connection, $sql);

        if($result)
        {
            if($result -> num_rows > 0)
            {
                while($row = mysqli_fetch_array($result))
                {
                    $progressionId = $row['progressionId'];
                    $lastChallenge = $row['lastChallengeId'];
                    $score = $row['score'];
                }
            }
        }
        echo "Last challenge : ".$lastChallenge;
        echo "\n";
        echo "Score".$score;
        echo "\n";
        return $progressionId;
    }

/*
    function GetCompletion($con, $id, $scenario, $chapter)
    {
        $query = "SELECT * FROM ontomatchgame.Progression INNER JOIN ontomatchgame.Session WHERE ontomatchgame.Progression.sessionId = ontomatchgame.Session.sessionId";
        $progressions = mysqli_query($con,$query);

        if ($progressions)
        {
            if ($progressions->num_rows > 0)
            {
                while ($row = mysqli_fetch_array($progressions))
                { 
                    if($row["historyId"] == $id)
                    {
                        if($row['scenarioId'] == "1" && $row['lastChapterId'] == "1")
                        {
                            echo "UserId->".$userId;
                            echo"\n";
                            $progressionId = $row['progressionId'];
                            echo "ProgressionId is : ".$row['progressionId'];
                            echo"\n";
                            break;
                        }
                    }
                }
            }
            else
            {
                ReturnEmptyString();
            }
        }
    }

*/

    /*

    //Inner join Session-Progression sur sessionId
    $query = "SELECT * FROM ontomatchgame.Progression INNER JOIN ontomatchgame.Session WHERE ontomatchgame.Progression.sessionId = ontomatchgame.Session.sessionId";
    $progressions = mysqli_query($connection,$query);

    if ($progressions)
    {
        if ($progressions->num_rows > 0)
        {
            while ($row = mysqli_fetch_array($progressions))
            { 
                if($row["historyId"] == $userId)
                {
                    if($row['scenarioId'] == "1" && $row['lastChapterId'] == "1")
                    {
                        echo "UserId->".$userId;
                        echo"\n";
                        $progressionId = $row['progressionId'];
                        echo "ProgressionId is : ".$row['progressionId'];
                        echo"\n";
                        break;
                    }
                }
            }
        }
        else
        {
            ReturnEmptyString();
        }
    }


    $query = "SELECT * FROM ontomatchgame.Progression WHERE ontomatchgame.Progression.progressionId = $progressionId";
    $completionScore = mysqli_query($connection,$query);

    if ($completionScore)
    {
        echo "GOT completion";
        echo"\n";

        if ($completionScore->num_rows > 0)
        {
            while ($row = mysqli_fetch_array($completionScore))
            { 
                echo "ChapterId : ".$row['chapterId'];
                echo"\n";
                echo "Score : ".$row['score'];
            }
        }
        else
        {
            ReturnEmptyString();
        }
    }
    else
    {
        echo "PB with query";
    }
*/

?>


























/*
    class ScenarioCompletions
    {
      public $Completions;

      public function __construct($list) 
      {
          $this->Completions = $list;
      }
    }

    $completionRates = array(
      array(1, 2, 3, 4, 5),
      array(11, 22, 33, 44 , 55),
      array(111, 222, 333, 444, 555),
      array(1111, 2222, 3333, 4444, 5555),
      array(11111, 22222, 33333, 44444, 55555)
    );
    $result = new ScenarioCompletions($completionRates);

    // echo "UserId = ".$_GET['UserId'];
    // echo<br>;
    // echo "Scenario name = ".$_GET['ScenarioName'];

    echo json_encode($result);
    
/*
    include("connect.php");

    if (!$connection->ping()) {
      printf ("Error: %s\n", $connection->error);
    }

    //DTO down, bool result
    class DoesExists
    {
      public $doesExist;

      public function __construct($b) 
      {
          $this->doesExist = $b;
      }
    }

    //Get Id of scenario and chapter



    //Search for email in DB.UserAccount
    $query = "SELECT * FROM Session WHERE scenarioId='".$_GET["email"]."'";    
    $result = $connection->query($query);

    if ($result->num_rows > 0) {//Email found
          $result = new DoesExists(true);
          echo json_encode($result);
      } else {
        $result = new DoesExists(false);
        echo json_encode($result);
    } 

    mysqli_close($connection);
    */
?>
