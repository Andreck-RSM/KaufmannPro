var KaufmannPro = (function () {

    /* =========================
       Submenü Definition
       ========================= */
    var submenuItems = {
        "Mandanten": ["Übersicht", "Anlegen", "Bearbeiten", "Archiv", "Einstellungen"],
        "Stammdaten": ["Kunden", "Lieferanten", "Produkte", "Steuern", "Kategorien"],
        "Auftrag": ["Neu", "Liste", "Status", "Freigabe", "Archiv"],
        "Bestellungen": ["Neu", "Liste", "Status", "Wareneingang", "Archiv"],
        "Finanzen": ["Rechnungen", "Offene Posten", "Konten", "Berichte", "Export"],
        "Zahlungen": ["Überweisungen", "Lastschriften", "Gutschriften", "Historie"],
        "Auswertungen": ["Dashboard", "Reports", "Statistiken", "Export"],
        "Dienste": ["Service 1", "Service 2", "Service 3"],
        "Extras": ["Option 1", "Option 2"],
        "Ansicht": ["Fenster 1", "Fenster 2"]
    };

    /* =========================
       Icons (oben + links)
       ========================= */
    var mainMenuIcons = {
        "Mandanten": "user",
        "Stammdaten": "preferences",
        "Auftrag": "edit",
        "Bestellungen": "cart",
        "Finanzen": "money",
        "Zahlungen": "card",
        "Auswertungen": "chart",
        "Dienste": "toolbox",
        "Extras": "fields",
        "Ansicht": "pinmap"
    };

    /* =========================
       Menü Builder
       ========================= */

    function buildCollapsedMenu() {
        return Object.keys(submenuItems).map(m => ({
            text: m,
            icon: mainMenuIcons[m] || "folder",
            expanded: false,
            items: submenuItems[m].map(x => ({ text: x })) // 👈 echte Kinder
        }));
    }

    function buildExpandedMenu(active) {
        return Object.keys(submenuItems).map(m => ({
            text: m,
            icon: mainMenuIcons[m] || "folder",
            expanded: m === active,
            items: submenuItems[m].map(x => ({ text: x }))
        }));
    }

    /* =========================
       Events
       ========================= */

    function onMainMenuClick(e) {
        var menuInstance = $("#mainMenu").dxMenu("instance");
        var tree = $("#submenuTree").dxTreeView("instance");

        // 🔹 aktiven Menüpunkt setzen (auch Home!)
        if (menuInstance) {
            menuInstance.option("selectedItem", e.itemData);
        }

        if (!tree) return;

        // 🔹 HOME über name erkennen (Icon-only)
        if (e.itemData.name === "home") {
            tree.option("items", buildCollapsedMenu());
            return;
        }

        // 🔹 normale Hauptmenüs
        var menu = e.itemData.text;
        if (submenuItems[menu]) {
            tree.option("items", buildExpandedMenu(menu));
        }
    }

    function onSubmenuClick(e) {
        console.log("Submenü:", e.itemData.text);
    }

    /* =========================
       Init
       ========================= */

    function init() {
        var tree = $("#submenuTree").dxTreeView("instance");
        if (!tree) {
            setTimeout(init, 50);
            return;
        }
        tree.option("items", buildCollapsedMenu());
    }

    $(init);

    return {
        onMainMenuClick,
        onSubmenuClick
    };

})();
