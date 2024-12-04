<?php

if (isset($_POST['RESULT'])) {
    $result = $_POST['RESULT']; // Retrieve RESULT from POST
} elseif (isset($_GET['RESULT'])) {
    $result = $_GET['RESULT']; // Retrieve RESULT from GET
} else {
    $result = null; // RESULT is not set
}

// Use the $result variable as needed
if ($result !== null) {
    echo "The RESULT value is: " . htmlspecialchars($result);
} else {
    echo "RESULT is not set.";
}
