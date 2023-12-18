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
| ASPNETCORE_ADLER_ADLERENGINEURL | Die URL des AdLer-Engine-Systems, mit dem AdLer interagiert. | Nein         | Ja                         |


## MariaDB Datenbankanforderung

Das AdLer Backend erfordert eine MariaDB-Datenbank zur korrekten Funktion. MariaDB ist ein relationales Open-Source-Datenbankmanagementsystem, das als Austauschkompatibel mit MySQL bekannt ist.

Die erforderlichen Umgebungsvariablen, die die Datenbank betreffen, müssen entsprechend konfiguriert werden, da der Verbindungsstring für MariaDB mit diesen Variablen erstellt wird. Dies beinhaltet `ASPNETCORE_DBPASSWORD`, `ASPNETCORE_DBUSER`, `ASPNETCORE_DBNAME`, `ASPNETCORE_DBHOST` und `ASPNETCORE_DBPORT`.

Stellen Sie sicher, dass diese Variablen korrekt eingestellt sind, um eine erfolgreiche Verbindung mit der MariaDB-Datenbank zu gewährleisten.


## API Endpunkte
Eine Swagger (OpenApi) Dokumentation sowie eine Postman Collection der API Endpunkte ist unter `/API Specification` verfügbar.
Zusätzlich kann ein Zugang zu der, immer aktuellen, Postman-Collection für das Backend von Philipp Geirhos angefragt werden. Dazu wird eine aktuelle Postman Version benötigt.
Auch muss dafür ein kostenloser Account angelegt werden. 
### Health Endpoint
Das AdLer-Backend bietet einen Health-Check-Endpoint an, der zur Überwachung des Systemzustands verwendet wird. Sie können eine GET-Anfrage an `/api/health` senden, um den Gesundheitsstatus des Servers zu überprüfen. Wenn der Server ordnungsgemäß funktioniert, wird das System die Nachricht "healthy" zurückgeben.

## Abhänigkeiten

| Backend Version | [AMG Version](https://github.com/ProjektAdLer/Autorentool) | [Moodle-Plugin Version](https://github.com/Glutamat42/moodle-docker) |
|-----------------|------------------------------------------------------------|----------------------------------------------------------------------|
| 2.0.0           | 2.x                                                        | 2.x                                                                  |
| 2.0.1           | 2.x                                                        | 2.x                                                                  |
| 2.0.2           | 2.x                                                        | 2.x                                                                  |
| 2.1.0           | 2.x  (Story ist Optional)                                  | 2.x (not needed)                                                     |

Zu jeden neuen Release wird diese Tabelle aktualisiert.
