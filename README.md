# Prosjekt Del-2:
## Dato: 2023-12-09 - Oppstart
- Et nytt prosjekt ble etablert med Tag v1.0.0.

## Dato: 2023-12-09 - Fjerning av kommentarer:
1. Branch "feature/Comment_delete".  
Det implementeres hovedfunksjonalitet knyttet til fjerning av kommentarer.
Bare bruker som eier denne kommentaren kan fjerne den. Delete knappen er bare tilgjengelig til denne brukeren.
2. Tag v1.0.1.

## Dato: 2023-12-09 - Pop-up dialog boks ved fjerning av kommentarer:
1. Branch "feature/Comment_delete_confirmation".  
Her tas det i bruk en MudBlazor Dialog funksjonalitet for å vise en simple pop-up dialog boks ved fjerning av kommentar. 
2. Tag v1.0.2.

## Dato: 2023-12-10 - Varsling og oppdatering av alle brukere ved fjerning av kommentar:
1. Branch "feature/Comment_delete_signalR".  
Eksisterende signalR funksjonalitet for kommentarer er oppdatert. Nå kan alle brukere få varsel og oppdatert page ved fjerning av kommentar.  
2. Tag v1.0.3.

## Dato: 2023-12-14 - Forbedring av test coverage:
1. Branch "feature/Test_coverage_improvement.  
Antall tester etter forbedring er over 100 stk.
Kort beskrivelse av hva som er gjort:  
1.1 DataAccess/Repository tester. Test coverage på 95%.  
1.2 DataAccess/Data. Test coverage på 96%.  
1.3 Api/Controllers. Test coverage på 92%.  
1.4 Api/Mapping. Test coverage på 100%.  
1.5 Common/Entity. Test coverage på 82%.  
1.6 Common/Dto. Test coverage på 97%.  
1.7 Server/Service. Test coverage på 68%.

2. Tag v1.0.4.

## Dato: 2023-12-14 - Etablerer en release branch som skal holde siste oppdateringer.





