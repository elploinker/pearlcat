﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pearlcat;

public class ObjectAddon : UpdatableAndDeletable, IDrawable
{
    public static ConditionalWeakTable<PhysicalObject, ObjectAddon> ObjectsWithAddon = new();

    public readonly WeakReference<AbstractPhysicalObject> AbstractObject;

    public ObjectAddon(AbstractPhysicalObject abstractObject)
    {
        AbstractObject = new WeakReference<AbstractPhysicalObject>(abstractObject);

        ObjectsWithAddon.Add(abstractObject.realizedObject, this);
        abstractObject.realizedObject.room.AddObject(this);
    }



    public override void Update(bool eu)
    {
        base.Update(eu);

        if (!AbstractObject.TryGetTarget(out var abstractObject)
            || abstractObject.slatedForDeletion
            || abstractObject.realizedObject == null
            || abstractObject.realizedObject.slatedForDeletetion)
            Destroy();

        if (slatedForDeletetion)
            RemoveFromRoom();
    }


    public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        int spriteIndex = 0;

        // Assign Sprite Indexes
        haloSprite = spriteIndex++;

        symbolSpriteSpear = spriteIndex++;
        symbolSpriteRage = spriteIndex++;
        symbolSpriteRevive = spriteIndex++;
        symbolSpriteShield = spriteIndex++;
        symbolSpriteAgility = spriteIndex++;
        symbolSpriteCamo = spriteIndex++;

        sLeaser.sprites = new FSprite[spriteIndex];

        // Create Sprites
        sLeaser.sprites[haloSprite] = new("LizardBubble6", true);

        sLeaser.sprites[symbolSpriteSpear] = new("BigGlyph2", true);
        sLeaser.sprites[symbolSpriteRage] = new("BigGlyph6", true);
        sLeaser.sprites[symbolSpriteRevive] = new("BigGlyph10", true);
        sLeaser.sprites[symbolSpriteShield] = new("BigGlyph11", true);
        sLeaser.sprites[symbolSpriteAgility] = new("BigGlyph8", true);
        sLeaser.sprites[symbolSpriteCamo] = new("BigGlyph12", true);

        AddToContainer(sLeaser, rCam, null!);
    }

    public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
    {
        sLeaser.RemoveAllSpritesFromContainer();

        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            var sprite = sLeaser.sprites[i];
            
            rCam.ReturnFContainer("Background").AddChild(sprite);
        }
    }

    public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette) { }


    PhysicalObject? parent;
    FSprite? parentSprite;

    public void ParentGraphics_DrawSprites(PhysicalObject self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        parent = self;
        parentSprite = sLeaser.sprites.FirstOrDefault();
    }


    public bool drawHalo = false;
    public int haloSprite;

    public float haloScale = 0.75f;
    public float haloAlpha = 0.5f;
    public Color haloColor = Color.white;

    public bool drawSymbolSpear;
    public bool drawSymbolRage;
    public bool drawSymbolRevive;
    public bool drawSymbolShield;
    public bool drawSymbolAgility;
    public bool drawSymbolCamo;

    public int symbolSpriteSpear;
    public int symbolSpriteRage;
    public int symbolSpriteRevive;
    public int symbolSpriteShield;
    public int symbolSpriteAgility;
    public int symbolSpriteCamo;

    public float symbolScale = 0.85f;
    public float symbolAlpha = 0.75f;
    public Color symbolColor = Color.white;

    public Vector2 symbolOffset = new(17.5f, 10.0f);


    public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (parent == null || parentSprite == null) return;

        if (slatedForDeletetion || rCam.room != room || parent.room != room)
        {
            sLeaser.CleanSpritesAndRemove();
            return;
        }

        var halo = sLeaser.sprites[haloSprite];
        halo.isVisible = drawHalo;
        halo.SetPosition(parentSprite.GetPosition());
        halo.scale = haloScale;
        halo.alpha = haloAlpha;
        halo.color = haloColor;


        var symbolSpear = sLeaser.sprites[symbolSpriteSpear];
        symbolSpear.SetPosition(parentSprite.GetPosition() + symbolOffset);
        symbolSpear.isVisible = drawSymbolSpear;
        symbolSpear.scale = symbolScale;
        symbolSpear.alpha = symbolAlpha;
        symbolSpear.color = symbolColor;

        var symbolRage = sLeaser.sprites[symbolSpriteRage];
        symbolRage.SetPosition(parentSprite.GetPosition() + symbolOffset);
        symbolRage.isVisible = drawSymbolRage;
        symbolRage.scale = symbolScale;
        symbolRage.alpha = symbolAlpha;
        symbolRage.color = symbolColor;

        var symbolRevive = sLeaser.sprites[symbolSpriteRevive];
        symbolRevive.SetPosition(parentSprite.GetPosition() + symbolOffset);
        symbolRevive.isVisible = drawSymbolRevive;
        symbolRevive.scale = symbolScale;
        symbolRevive.alpha = symbolAlpha;
        symbolRevive.color = symbolColor;

        var symbolShield = sLeaser.sprites[symbolSpriteShield];
        symbolShield.SetPosition(parentSprite.GetPosition() + symbolOffset);
        symbolShield.isVisible = drawSymbolShield;
        symbolShield.scale = symbolScale;
        symbolShield.alpha = symbolAlpha;
        symbolShield.color = symbolColor;

        var symbolAgility = sLeaser.sprites[symbolSpriteAgility];
        symbolAgility.SetPosition(parentSprite.GetPosition() + symbolOffset);
        symbolAgility.isVisible = drawSymbolAgility;
        symbolAgility.scale = symbolScale;
        symbolAgility.alpha = symbolAlpha;
        symbolAgility.color = symbolColor;

        var symbolCamo = sLeaser.sprites[symbolSpriteCamo];
        symbolCamo.SetPosition(parentSprite.GetPosition() + symbolOffset);
        symbolCamo.isVisible = drawSymbolCamo;
        symbolCamo.scale = symbolScale;
        symbolCamo.alpha = symbolAlpha;
        symbolCamo.color = symbolColor;
    }
}
