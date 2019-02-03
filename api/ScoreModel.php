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

    public function addScore($user_id, $score) {
    	$connection = $this -> connect_db();
    	$n = mysqli_real_escape_string($connection, $user_id);
   		$a = mysqli_real_escape_string($connection, $score);
    	$query = "INSERT INTO `scores` (`user_id`, `score`) VALUES ('$n', '$a');";
    	mysqli_query($connection, $query);
    	$this -> disconnect_db($connection);
    }

    public function getScores() {
    	$connection = $this -> connect_db();
    	$query = "SELECT * from `scores`;";
    	$result = mysqli_query($connection, $query);
    	while($query_data = mysqli_fetch_row($result)) {
			error_log(json_encode($query_data));
		}
    	$this -> disconnect_db($connection);
    }

    public function getScoreById($user_id) {
    	$user_id = mysqli_real_escape_string($connection, $user_id);
    	$connection = $this -> connect_db();
    	$query = "SELECT * from `scores` WHERE `user_id` = $user_id;";
    	$result = mysqli_query($connection, $query);
    	while($query_data = mysqli_fetch_row($result)) {
			error_log(json_encode($query_data));
		}
    	$this -> disconnect_db($connection);
    }

}

?>