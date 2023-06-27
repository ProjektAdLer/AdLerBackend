# Konfiguration des AdLer Backends

Dieses Dokument enthält Informationen zur Konfiguration des AdLer Backends. Es ist wichtig, diese Einstellungen korrekt vorzunehmen, um die volle Funktionalität des AdLer-Systems zu gewährleisten.

## Umgebungsvariablen

Die folgende Tabelle listet alle Umgebungsvariablen auf, die konfiguriert werden müssen, einschließlich ihrer Beschreibung und ob sie erforderlich sind oder nur in der Produktionsumgebung benötigt werden.

| Variablenname                | Beschreibung                                       | Erforderlich | Erforderlich in Produktion |
|------------------------------|---------------------------------------------------|--------------|----------------------------|
| ASPNETCORE_ENVIRONMENT       | Die Umgebung, in der die Anwendung läuft.           | Ja           | N/A                        |
| ASPNETCORE_ADLER_MOODLEHOST  | Der Host des Moodle-Systems, mit dem AdLer interagiert. | ja           | Nein                       |
| ASPNETCORE_ADLER_MOODLEURL   | Die URL des Moodle-Systems, mit dem AdLer interagiert. | Ja           | Ja                         |
| ASPNETCORE_DBPASSWORD        | Passwort für die Datenbank.                        | Nein         | Ja                         |
| ASPNETCORE_DBUSER            | Benutzername für die Datenbank.                     | Nein         | Ja                         |
| ASPNETCORE_DBNAME            | Name der Datenbank.                                | Nein         | Ja                         |
| ASPNETCORE_DBHOST            | Host der Datenbank.                                | Nein         | Ja                         |
| ASPNETCORE_DBPORT            | Port der Datenbank.                                | Nein         | Ja                         |
| ASPNETCORE_ADLER_HTTPPORT    | Der HTTP-Port, auf dem AdLer lauscht. Standardwert ist 80. | Nein         | Nein                       |

## Integration mit Moodle und Docker Compose

Um ein voll funktionsfähiges AdLer-System zu haben, müssen das Moodle-System und andere Systeme in die Docker-Compose-Datei integriert werden. Weitere Informationen zur Einrichtung und Verwendung von Docker Compose finden Sie in der Docker Compose Dokumentation.

Bitte beachten Sie, dass ein korrekter Docker Compose-Workflow und die Integration des Moodle-Systems entscheidend sind, um das AdLer-System zum Laufen zu bringen.

## Bereitstellung des Backends

Die vollständige Dokumentation zur Infrastruktur und zur Bereitstellung des Backends finden Sie in der entsprechenden Infrastruktur-Dokumentation. Bitte folgen Sie diesen Anweisungen genau, um das Backend korrekt zu bereitstellen.

Beachten Sie, dass die obigen Umgebungsvariablen korrekt konfiguriert sein müssen, bevor das Backend bereitgestellt wird.

## MariaDB Datenbankanforderung

Das AdLer Backend erfordert eine MariaDB-Datenbank zur korrekten Funktion. MariaDB ist ein relationales Open-Source-Datenbankmanagementsystem, das als Austauschkompatibel mit MySQL bekannt ist.

Die erforderlichen Umgebungsvariablen, die die Datenbank betreffen, müssen entsprechend konfiguriert werden, da der Verbindungsstring für MariaDB mit diesen Variablen erstellt wird. Dies beinhaltet `ASPNETCORE_DBPASSWORD`, `ASPNETCORE_DBUSER`, `ASPNETCORE_DBNAME`, `ASPNETCORE_DBHOST` und `ASPNETCORE_DBPORT`.

Stellen Sie sicher, dass diese Variablen korrekt eingestellt sind, um eine erfolgreiche Verbindung mit der MariaDB-Datenbank zu gewährleisten.

## Swagger-Konfiguration im Projekt-Root

Im Root-Verzeichnis des Projekts finden Sie eine Swagger-Konfigurationsdatei.

Die Swagger-Konfigurationsdatei definiert die Struktur der API und enthält eine Liste aller verfügbaren Endpunkte, zusammen mit den unterstützten HTTP-Methoden und den erwarteten Strukturen für Anfragen und Antworten.

Es ist wichtig zu beachten, dass jede Änderung an den Endpunkten oder ihrer Funktionalität in dieser Datei reflektiert werden sollte, um sicherzustellen, dass die API-Dokumentation stets aktuell und korrekt ist.
