﻿using UnityEngine;
using System.Collections.Generic;

namespace Chipper.Rendering
{
    public class SpriteRendererConversionSystem : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            var spriteLoader = SpriteLoader.Main;
            var poolInfo = RenderSettings.Main.PoolInfo;
            var materials = new HashSet<Material>();

            foreach(var pool in poolInfo)
                materials.Add(pool.Material);

            Entities.ForEach((SpriteRenderer renderer) =>
            {
                var entity = GetPrimaryEntity(renderer);
                var material = renderer.sharedMaterial;
                var gameObject = renderer.gameObject;

                Debug.Assert(materials.Contains(material), $"( {material.name} : {gameObject.name}) => Material has no associated object pool. You need to create one inside `RenderSettings` ");

                DstEntityManager.AddComponentData(entity, spriteLoader.GetSpriteID(renderer.sprite));

                DstEntityManager.AddComponentData(entity, new RenderInfo
                {
                    Color = renderer.color,
                    IsDefaultDirectionRight = !renderer.flipX,
                    Layer = gameObject.layer,
                    SortingLayer = renderer.sortingLayerID,                     
                });

                DstEntityManager.AddComponentData(entity, new RenderFlipState
                {
                    flipX = renderer.flipX,
                    flipY = renderer.flipY,
                });

                DstEntityManager.AddSharedComponentData(entity, new MaterialInfo
                {
                    MaterialID = material.GetHashCode(),
                });
            });
        }
    }
}
