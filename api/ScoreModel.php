<?php

class ScoreModel {

    public function __construct() {

    }

    private function connect_db() {
        include "/var/www/inc/dbinfo.inc";
        $connection = mysqli_connect(DB_SERVER, DB_USERNAME, DB_PASSWORD);

        error_log("server: " . DB_SERVER);
        error_log("username: " . DB_USERNAME);

        if (mysqli_connect_errno()) {
            echo "Failed to connect to MySQL: " . mysqli_connect_error();
            die;
        }
        mysqli_select_db($connection, DB_DATABASE);
        return $connection;
    }

    private function disconnect_db($connection) {
        mysqli_close($connection);
    }

    public function addScore($user_id, $username, $score) {
        $connection = $this -> connect_db();
        $n = mysqli_real_escape_string($connection, $user_id);
        $a = mysqli_real_escape_string($connection, $score);
        $name = mysqli_real_escape_string($connection, $username);
        $query = "INSERT INTO `scores` (`user_id`, `username`, `score`) VALUES ('$n', '$name', '$a');";
        mysqli_query($connection, $query);
        $this -> disconnect_db($connection);
    }

    public function getScores($limit) {
        $connection = $this -> connect_db();
        $query = "SELECT * from `scores` ORDER BY `score` DESC LIMIT $limit;";
        $result = mysqli_query($connection, $query);
        $ret = array();
        while($query_data = mysqli_fetch_row($result)) {
            array_push($ret, array(
                'id' => $query_data[0],
                'user_id' => $query_data[1],
                'score' => $query_data[2]
            ));
        }
        $this -> disconnect_db($connection);
        return $ret;
    }

    public function getScoreById($user_id, $limit) {
        $connection = $this -> connect_db();    
        $user_id = mysqli_real_escape_string($connection, $user_id);
            $query = "SELECT * from `scores` WHERE `user_id` = $user_id ORDER BY `score` DESC LIMIT $limit;";
        $result = mysqli_query($connection, $query);
        $ret = array();
            while($query_data = mysqli_fetch_row($result)) {
            array_push($ret, array(
                'id' => $query_data[0],
                'user_id' => $query_data[1],
                'score' => $query_data[2]
            ));
        }
        $this -> disconnect_db($connection);
        return $ret;
    }

}

?>
