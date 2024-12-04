<?php
// Define the file path
$file_path = 'received_data.txt';

// Check if the file exists
if (file_exists($file_path)) {
    // Read the file into an array
    $fileContents = file($file_path);

    // Print each line
    foreach ($fileContents as $line) {
        echo "Line: " . $line . "<br>";
    }
} else {
    echo "The file does not exist.";
}
?>
