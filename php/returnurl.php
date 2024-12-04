<?php
// Loop through all GET parameters
if (!empty($_GET)) {
    echo "<h3>Received Parameters:</h3>";
    echo "<table border='1' cellpadding='5' cellspacing='0'>";
    echo "<tr><th>Parameter</th><th>Value</th></tr>";
    foreach ($_GET as $key => $value) {
        echo "<tr><td>" . htmlspecialchars($key) . "</td><td>" . htmlspecialchars($value) . "</td></tr>";
    }
    echo "</table>";
} else {
    echo "No parameters received.";
}
?>
