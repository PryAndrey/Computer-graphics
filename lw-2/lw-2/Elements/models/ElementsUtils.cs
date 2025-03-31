namespace Elements.models;

public enum ElementType
{
    Air,
    Alcohol,
    Bacteria,
    Cloud,
    Crater,
    Dust,
    Earth,
    Electricity,
    Energy,
    Explosion,
    Fire,
    Geyser,
    Gunpowder,
    Hydrogen,
    Life,
    Lava,
    Metal,
    MolotovCoctail,
    Mud,
    Oxygen,
    Ozone,
    Pressure,
    Rain,
    RattlesnakeGas,
    Rust,
    Shockwave,
    Steam,
    SteamBoiler,
    Stone,
    Storm,
    Swamp,
    Tsunami,
    Vodka,
    Volcano,
    Water
}

public static class ElementCombinations
{
    public static readonly Dictionary<(ElementType, ElementType), ElementType> CombinationResults = new()
    {
        { (ElementType.Fire, ElementType.Water), ElementType.Steam },
        { (ElementType.Fire, ElementType.Earth), ElementType.Lava },
        { (ElementType.Water, ElementType.Air), ElementType.Cloud },
        { (ElementType.Water, ElementType.Earth), ElementType.Mud },
        { (ElementType.Water, ElementType.Mud), ElementType.Swamp },
        { (ElementType.Earth, ElementType.Earth), ElementType.Pressure },
        { (ElementType.Earth, ElementType.Pressure), ElementType.Ozone },

        { (ElementType.Fire, ElementType.Gunpowder), ElementType.Explosion },
        { (ElementType.Fire, ElementType.Air), ElementType.Energy },
        { (ElementType.Fire, ElementType.Alcohol), ElementType.MolotovCoctail },
        { (ElementType.Fire, ElementType.Metal), ElementType.Electricity },

        { (ElementType.Alcohol, ElementType.Pressure), ElementType.Hydrogen },
        { (ElementType.Air, ElementType.Pressure), ElementType.Oxygen },
        { (ElementType.Water, ElementType.Pressure), ElementType.Geyser },
        { (ElementType.Steam, ElementType.Pressure), ElementType.SteamBoiler },
        { (ElementType.Water, ElementType.Energy), ElementType.Life },
        { (ElementType.Water, ElementType.Bacteria), ElementType.Life },
        { (ElementType.Water, ElementType.Alcohol), ElementType.Vodka },

        { (ElementType.Air, ElementType.Dust), ElementType.Storm },
        { (ElementType.Air, ElementType.Energy), ElementType.Electricity },
        { (ElementType.Air, ElementType.Ozone), ElementType.Cloud },

        { (ElementType.Lava, ElementType.Water), ElementType.Stone },
        { (ElementType.Mud, ElementType.Pressure), ElementType.Stone },
        { (ElementType.Lava, ElementType.Pressure), ElementType.Volcano },

        { (ElementType.Cloud, ElementType.Pressure), ElementType.Rain },
        { (ElementType.Cloud, ElementType.Electricity), ElementType.Storm },

        { (ElementType.Explosion, ElementType.Air), ElementType.Shockwave },
        { (ElementType.Explosion, ElementType.Water), ElementType.Tsunami },
        { (ElementType.Explosion, ElementType.Earth), ElementType.Crater },

        { (ElementType.Oxygen, ElementType.Hydrogen), ElementType.Water },
        { (ElementType.Metal, ElementType.Oxygen), ElementType.Rust },

        { (ElementType.Swamp, ElementType.Bacteria), ElementType.RattlesnakeGas },
        { (ElementType.Swamp, ElementType.Life), ElementType.Alcohol },
        { (ElementType.Alcohol, ElementType.Fire), ElementType.MolotovCoctail },
        { (ElementType.Vodka, ElementType.Bacteria), ElementType.Life },
    };

    public static ElementType? GetCombinationResult(ElementType element1, ElementType element2)
    {
        if (CombinationResults.TryGetValue((element1, element2), out ElementType result) ||
            CombinationResults.TryGetValue((element2, element1), out result))
        {
            return result;
        }

        return null;
    }
    
    private static readonly Dictionary<ElementType, string> typeStrings = new()
    {
        { ElementType.Fire, "Fire" },
        { ElementType.Earth, "Earth" },
        { ElementType.Water, "Water" },
        { ElementType.Air, "Air" },
        { ElementType.Alcohol, "Alcohol" },
        { ElementType.Bacteria, "Bacteria" },
        { ElementType.Cloud, "Cloud" },
        { ElementType.Crater, "Crater" },
        { ElementType.Dust, "Dust" },
        { ElementType.Electricity, "Electricity" },
        { ElementType.Energy, "Energy" },
        { ElementType.Explosion, "Explosion" },
        { ElementType.Geyser, "Geyser" },
        { ElementType.Gunpowder, "Gunpowder" },
        { ElementType.Hydrogen, "Hydrogen" },
        { ElementType.Lava, "Lava" },
        { ElementType.Life, "Life" },
        { ElementType.Metal, "Metal" },
        { ElementType.MolotovCoctail, "MolotovCoctail" },
        { ElementType.Mud, "Mud" },
        { ElementType.Oxygen, "Oxygen" },
        { ElementType.Ozone, "Ozone" },
        { ElementType.Pressure, "Pressure" },
        { ElementType.Rain, "Rain" },
        { ElementType.RattlesnakeGas, "RattlesnakeGas" },
        { ElementType.Rust, "Rust" },
        { ElementType.Shockwave, "Shockwave" },
        { ElementType.Steam, "Steam" },
        { ElementType.SteamBoiler, "SteamBoiler" },
        { ElementType.Stone, "Stone" },
        { ElementType.Storm, "Storm" },
        { ElementType.Swamp, "Swamp" },
        { ElementType.Tsunami, "Tsunami" },
        { ElementType.Vodka, "Vodka" },
        { ElementType.Volcano, "Volcano" },
    };
    
    public static string ToString(ElementType elementType)
    {
        return typeStrings.TryGetValue(elementType, out var name) ? name : "--";
    }
}

public static class ElementIdGenerator
{
    private static int _nextId = 0;

    public static int GetNextId()
    {
        return _nextId++;
    }

    public static void Reset()
    {
        _nextId = 0;
    }
}

public static class ElementImages
{
    private static readonly Dictionary<ElementType, string> _imagePaths = new()
    {
        { ElementType.Fire, "../../../images/Fire.png" },
        { ElementType.Earth, "../../../images/Earth.png" },
        { ElementType.Water, "../../../images/Water.png" },
        { ElementType.Air, "../../../images/Air.png" },
        { ElementType.Alcohol, "../../../images/Alcohol.png" },
        { ElementType.Bacteria, "../../../images/Bacteria.png" },
        { ElementType.Cloud, "../../../images/Cloud.png" },
        { ElementType.Crater, "../../../images/Crater.png" },
        { ElementType.Dust, "../../../images/Dust.png" },
        { ElementType.Electricity, "../../../images/Electricity.png" },
        { ElementType.Energy, "../../../images/Energy.png" },
        { ElementType.Explosion, "../../../images/Explosion.png" },
        { ElementType.Geyser, "../../../images/Geyser.png" },
        { ElementType.Gunpowder, "../../../images/Gunpowder.png" },
        { ElementType.Hydrogen, "../../../images/Hydrogen.png" },
        { ElementType.Lava, "../../../images/Lava.png" },
        { ElementType.Life, "../../../images/Life.png" },
        { ElementType.Metal, "../../../images/Metal.png" },
        { ElementType.MolotovCoctail, "../../../images/MolotovCoctail.png" },
        { ElementType.Mud, "../../../images/Mud.png" },
        { ElementType.Oxygen, "../../../images/Oxygen.png" },
        { ElementType.Ozone, "../../../images/Ozone.png" },
        { ElementType.Pressure, "../../../images/Pressure.png" },
        { ElementType.Rain, "../../../images/Rain.png" },
        { ElementType.RattlesnakeGas, "../../../images/RattlesnakeGas.png" },
        { ElementType.Rust, "../../../images/Rust.png" },
        { ElementType.Shockwave, "../../../images/Shockwave.png" },
        { ElementType.Steam, "../../../images/Steam.png" },
        { ElementType.SteamBoiler, "../../../images/SteamBoiler.png" },
        { ElementType.Stone, "../../../images/Stone.png" },
        { ElementType.Storm, "../../../images/Storm.png" },
        { ElementType.Swamp, "../../../images/Swamp.png" },
        { ElementType.Tsunami, "../../../images/Tsunami.png" },
        { ElementType.Vodka, "../../../images/Vodka.png" },
        { ElementType.Volcano, "../../../images/Volcano.png" },
    };

    public static string GetImagePath(ElementType elementType)
    {
        return _imagePaths.TryGetValue(elementType, out var path) ? path : "../../images/None.png";
    }
}