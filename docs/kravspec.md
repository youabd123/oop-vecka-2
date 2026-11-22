# Kravspecifikation

## Problemet, med kundens egna ord

”Vi missar deadlines hela tiden, och ingen märker det förrän
kunden ringer och är arg.”

Teamet består av åtta personer. Appen visar dessutom uppgifter
som försenade redan under förfallodagen.

## Mål, och hur vi vet att vi har nått dem

- Nuläge – uppgifter markeras felaktigt som försenade samma dag.
- Mål – korrekt förseningsstatus enligt acceptanskriterierna.
- Tidsram – innan buggrättningen godkänns.
- Mätmetod – automatiserade tester med kriteriernas ID:n.

Målet är också färre missade deadlines. Nuläge och målvärde
behöver kompletteras. Enligt Övning 8 ska resultatet följas upp
med Sara efter fyra veckor med statistik från appen.

## Frågor vi ställde, och svaren

Intervjuanteckningarna behöver kompletteras.

I kursmaterialet säger Sara:
”Har jag sagt fredag så får det bli klart fredag kväll.”

Övning 5 anger att förfallodatum saknar klockslag och att
svensk tid, Europe/Stockholm, ska användas.

Öppna frågor:
- Hur många uppgifter blir klara i tid idag?
- Vilken förbättring vill Sara nå?
- Hur ska ändrade deadlines påverka statistiken?

## User stories med acceptanskriterier

### US-01 – Planera dagen

Som teammedlem vill jag se vad som behöver göras först,
så att jag kan planera arbetsdagen.

Följande sortering och felhantering är förslag.

- AK-1.1 Givet oavslutade uppgifter med olika förfallodatum,
  när arbetslistan öppnas, så visas tidigast förfallodatum först.
  Uppgifter utan datum visas sist och slutförda uppgifter utelämnas.
- AK-1.2 Givet att API-anropet misslyckas, när arbetslistan öppnas,
  så visas ett felmeddelande och möjlighet att försöka igen.
- AK-1.3 Givet en uppgift som har tagits bort, när användaren
  försöker markera den som klar, så svarar API:t med 404
  och appen informerar om att uppgiften saknas.

### US-02 – Korrekt förseningsstatus

Som teammedlem vill jag se vilka uppgifter som är försenade,
så att jag kan lita på listan.

- AK-2.1 Givet en oavslutad uppgift som förfaller idag,
  när klockan är 23.59 svensk tid, så är den inte försenad.
- AK-2.2 Givet en oavslutad uppgift som förföll igår,
  när listan hämtas idag i svensk tid, så är den försenad.
- AK-2.3 Givet en slutförd uppgift eller en uppgift utan
  förfallodatum, när försenade uppgifter hämtas,
  så finns uppgiften inte med.

### US-03 – Följa upp deadlines

Som teamledare vill jag se hur många uppgifter som blir klara
i tid, så att jag kan följa upp teamets leveranser.

- AK-3.1 Givet en oavslutad uppgift, när den markeras som klar,
  så sparas tidpunkten för slutförandet.
- AK-3.2 Givet uppgifter klara på förfallodagen, klara efter
  förfallodagen och oavslutade med passerat förfallodatum,
  när GET /api/todos/stats anropas, så redovisas respektive antal.
- AK-3.3 Givet att statistikunderlag saknas, när statistiken
  hämtas, så anges att underlag saknas i stället för 0 %.

Hur andelen klara i tid ska beräknas behöver fastställas.

## Ett use case med huvudflöde, alternativa flöden och undantag

### Planera dagen

Huvudflöde:
1. Teammedlemmen öppnar appen på morgonen.
2. Systemet hämtar arbetslistan.
3. Systemet sorterar uppgifterna enligt AK-1.1.
4. Systemet visar korrekt förseningsstatus enligt US-02.
5. Teammedlemmen väljer vad som ska göras först.

Alternativa flöden:
- Inget är försenat. Dagens och kommande uppgifter visas.
  Täcks av AK-1.1 och AK-2.1.
- En uppgift slutförs efter deadline. Slutförandet sparas,
  uppgiften försvinner ur arbetslistan och räknas som klar sent.
  Täcks av AK-1.1, AK-2.3, AK-3.1 och AK-3.2.

Undantagsflöden:
- API:t svarar inte. Felmeddelande och nytt försök erbjuds.
  Täcks av AK-1.2.
- Uppgiften har tagits bort. Användaren informeras.
  Täcks av AK-1.3.

## Icke-funktionella krav, med siffror

Förslag från lektionsmaterialet:
Listan ska laddas på under 200 ms vid åtta samtidiga användare.

Datamängd, testmiljö, mätmetod och tillåten felfrekvens
behöver fastställas innan kravet kan verifieras.

## Utanför scope, alltså det vi medvetet inte gör nu

Föreslagen avgränsning:
- Automatiska påminnelser.
- Tilldelning av uppgifter till personer.
- Kalenderintegrationer.
