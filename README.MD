# Konfiguration des AdLer Backends

## Umgebungsvariablen

Die folgende Tabelle listet alle Umgebungsvariablen auf, die konfiguriert werden müssen, einschließlich ihrer Beschreibung und ob sie erforderlich sind oder nur in der Produktionsumgebung benötigt werden.

| Variablenname                | Beschreibung                                       | Erforderlich | Erforderlich in Produktion |
|------------------------------|---------------------------------------------------|--------------|----------------------------|
| ASPNETCORE_ENVIRONMENT       | Die Umgebung, in der die Anwendung läuft.           | Ja           | N/A                        |
| ASPNETCORE_ADLER_MOODLEURL   | Die URL des Moodle-Systems, mit dem AdLer interagiert. | Ja           | Ja                         |
| ASPNETCORE_DBPASSWORD        | Passwort für die Datenbank.                        | Nein         | Ja                         |
| ASPNETCORE_DBUSER            | Benutzername für die Datenbank.                     | Nein         | Ja                         |
| ASPNETCORE_DBNAME            | Name der Datenbank.                                | Nein         | Ja                         |
| ASPNETCORE_DBHOST            | Host der Datenbank.                                | Nein         | Ja                         |
| ASPNETCORE_DBPORT            | Port der Datenbank.                                | Nein         | Ja                         |
| ASPNETCORE_ADLER_HTTPPORT    | Der HTTP-Port, auf dem AdLer lauscht. Standardwert ist 80. | Nein         | Nein                       |


## MariaDB Datenbankanforderung

Das AdLer Backend erfordert eine MariaDB-Datenbank zur korrekten Funktion. MariaDB ist ein relationales Open-Source-Datenbankmanagementsystem, das als Austauschkompatibel mit MySQL bekannt ist.

Die erforderlichen Umgebungsvariablen, die die Datenbank betreffen, müssen entsprechend konfiguriert werden, da der Verbindungsstring für MariaDB mit diesen Variablen erstellt wird. Dies beinhaltet `ASPNETCORE_DBPASSWORD`, `ASPNETCORE_DBUSER`, `ASPNETCORE_DBNAME`, `ASPNETCORE_DBHOST` und `ASPNETCORE_DBPORT`.

Stellen Sie sicher, dass diese Variablen korrekt eingestellt sind, um eine erfolgreiche Verbindung mit der MariaDB-Datenbank zu gewährleisten.


## Health Endpoint

Das AdLer-Backend bietet einen Health-Check-Endpoint an, der zur Überwachung des Systemzustands verwendet wird. Sie können eine GET-Anfrage an `/api/health` senden, um den Gesundheitsstatus des Servers zu überprüfen. Wenn der Server ordnungsgemäß funktioniert, wird das System die Nachricht "healthy" zurückgeben.