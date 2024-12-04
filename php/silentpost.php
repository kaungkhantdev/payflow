<?php
ini_set('display_errors', 1);
error_reporting(E_ALL);

// Get raw POST data from the input stream
$rawData = file_get_contents("php://input");

// Capture GET parameters
$getData = $_GET;

// Capture POST data
$postData = $_POST;

// Define the file path where you want to save the data
$file_path = 'received_data.php';

// Initialize the content variable
$content = "<?php\n\n/* Received Data Log */\n\n";

// Check if raw POST data was received
if (!empty($rawData)) {
    $content .= "// Raw Data:\n";
    $content .= "echo 'Raw Data: " . addslashes($rawData) . "';\n";
}

// Check if GET data was received
if (!empty($getData)) {
    $content .= "// GET Data:\n";
    $content .= "echo 'GET Data: " . addslashes(json_encode($getData)) . "';\n";
}

// Check if POST data was received
if (!empty($postData)) {
    $content .= "// POST Data:\n";
    $content .= "echo 'POST Data: " . addslashes(json_encode($postData)) . "';\n";
}

// Save the content to the PHP file
file_put_contents($file_path, $content);

// Display a success message
echo "The received data has been saved to received_data.php";
?>
