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

// Section for raw POST data
if (!empty($rawData)) {
    $content .= "/* Raw Data */\n";
    $content .= "echo 'Raw Data: ' . htmlspecialchars('" . addslashes($rawData) . "') . \"<br>\";\n\n";
}

// Section for GET data
if (!empty($getData)) {
    $content .= "/* GET Data */\n";
    $content .= "\$getData = " . var_export($getData, true) . ";\n";
    $content .= "echo 'GET Data: ' . htmlspecialchars(json_encode(\$getData, JSON_PRETTY_PRINT)) . \"<br>\";\n\n";
}

// Section for POST data
if (!empty($postData)) {
    $content .= "/* POST Data */\n";
    $content .= "\$postData = " . var_export($postData, true) . ";\n";
    $content .= "echo 'POST Data: ' . htmlspecialchars(json_encode(\$postData, JSON_PRETTY_PRINT)) . \"<br>\";\n\n";
}

// Save the content to the PHP file
file_put_contents($file_path, $content);

// Display a success message
echo "The received data has been saved to received_data.php";
?>
