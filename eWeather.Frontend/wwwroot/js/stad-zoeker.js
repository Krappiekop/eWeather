function initStadZoeker(stations) {
    const stadInput = document.getElementById("stadInput");
    const stadHidden = document.getElementById("stadHidden");
    const stadLijst = document.getElementById("stadLijst");

    function toonOpties(lijst) {
        stadLijst.innerHTML = "";

        lijst.forEach(s => {
            const item = document.createElement("li");
            item.textContent = s.Regio;
            item.addEventListener("click", () => {
                stadInput.value = s.Regio;
                stadHidden.value = s.Station;
                stadLijst.innerHTML = "";
                stadLijst.style.display = "none";
                stadInput.form.submit();
            });
            stadLijst.appendChild(item);
        });

        stadLijst.style.display = lijst.length > 0 ? "block" : "none";
    }

    stadInput.addEventListener("focus", () => {
        toonOpties(stations);
    });

    stadInput.addEventListener("input", () => {
        const zoekterm = stadInput.value.toLowerCase();
        const gefilterd = stations.filter(s => s.Regio.toLowerCase().includes(zoekterm));
        toonOpties(gefilterd);
    });

    document.addEventListener("click", (event) => {
        const zoeker = document.querySelector(".stad-zoeker");

        if (!zoeker.contains(event.target)) {
            stadLijst.style.display = "none";
        }
    });
}