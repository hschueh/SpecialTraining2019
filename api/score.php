<?php

require_once __DIR__ . '/ScoreModel.php';

function update_score($user_id, $score) {
	$score_model = new ScoreModel();
	$score_model -> addScore($user_id, $score);
	return '1';
}

function get_score($user_id, $limit = 10) {
	$score_model = new ScoreModel();
	if ($user_id == "") {
		$data = $score_model -> getScores($limit);
	} else {
		$data = $score_model -> getScoreById($user_id, $limit);
	}
	if (!empty($data)) {
		return array('result' => '1', 'data' => $data);
	} else {
		return array('result' => '0', 'message' => 'Error!');
	}
}

// $result = get_score();

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
	$user_id = "";
	$score = "";
	if (isset($_POST['user_id']) && is_numeric($_POST['user_id'])) {
		$user_id = $_POST['user_id'];
	}
	if (isset($_POST['score']) && is_numeric($_POST['score'])) {
		$score = $_POST['score'];
	}

	if ($user_id == "" || $score == "") {
		header("HTTP/1.1 400 Bad request!");
	} else {
		$result = update_score($user_id, $score);
		echo json_encode($result);
	}
} else if ($_SERVER['REQUEST_METHOD'] === 'GET') {
	$user_id = "";
	$limit = "";
	if (isset($_GET['user_id']) && is_numeric($_GET['user_id'])) {
		$user_id = $_GET['user_id'];
	}
	if (isset($_GET['limit']) && is_numeric($_GET['limit'])) {
		$limit = $_GET['limit'];
	}

	// default limit = 10;
	if ($limit == "") {
		$limit = "10";
	}

	if ($user_id == "") {
		$result = get_score("", $limit);
	} else {
		$result = get_score($user_id, $limit);
	}

	echo json_encode($result);
}

?>
