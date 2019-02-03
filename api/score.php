<?php

require_once __DIR__ . '/ScoreModel.php';

function update_score($user_id, $score) {
	$score_model = new ScoreModel();
	$score_model -> addScore($user_id, $score);
}

function get_score() {
	$score_model = new ScoreModel();
	$result = $score_model -> getScores();
}

// if ($_SERVER['REQUEST_METHOD'] === 'POST') {
// 	update_score();
// } else if ($_SERVER['REQUEST_METHOD'] === 'GET') {
// 	get_scores();
// }

?>