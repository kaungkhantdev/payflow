<?php
ini_set('display_errors', 1);
error_reporting(E_ALL);

// Get raw POST data from the input stream (this works only for POST requests)
$rawData = file_get_contents("php://input");

// Capture GET parameters
$getData = $_GET;

// Capture POST data (if available)
$postData = $_POST;

// Define the file path where you want to save the data
$file_path = 'received_data.txt';

// Check if raw POST data was received and save it
if (!empty($rawData)) {
    file_put_contents($file_path, "Received Raw Data: " . $rawData . "\n", FILE_APPEND);
}

// Check if GET data was received and save it
if (!empty($getData)) {
    file_put_contents($file_path, "Received GET Data: " . json_encode($getData) . "\n", FILE_APPEND);
}

// Check if POST data was received and save it
if (!empty($postData)) {
    file_put_contents($file_path, "Received POST Data: " . json_encode($postData) . "\n", FILE_APPEND);
}

// Check if no data was received
if (empty($rawData) && empty($getData) && empty($postData)) {
    echo "No data received.";
} else {
    echo "The received data has been saved to the file.";
}
?>
