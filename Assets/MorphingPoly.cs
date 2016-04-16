﻿using UnityEngine;
using System.Collections;

public enum MorphType {
    Shape,
    Movement,
    Rotation,
    Projectiles,
    Shields
}

public class MorphingPoly : Poly {

    Timer timer;

    [SerializeField]
    protected float morphTime = 10f;

    [SerializeField]
    protected float morphChance = .5f;

    protected override void init() {
        base.init();

        timer = new Timer();
    }

    protected override void tick() {
        base.tick();

        // If ample space, alive long enough
        if (timer.elapsedTime > morphTime) {
            AttemptMorph();
        }
    }

    public virtual void AttemptMorph() {
        if (Random.value > morphChance) {
            Morph(MorphType.Shape);
        }
    }

    public virtual void Morph(MorphType mtype) {
        switch (mtype) {
            case MorphType.Shape:
                switch (this.ptype) {
                    case PolyType.Hexagon:
                        this.ptype = PolyType.Octagon;
                        this.GetComponent<MeshFilter>().mesh = PolyTool.CreateOctagon();
                        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().mesh;
                        break;
                    case PolyType.Pentagon:
                        this.ptype = PolyType.Hexagon;
                        this.GetComponent<MeshFilter>().mesh = PolyTool.CreateHexagon();
                        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().mesh;
                        break;
                    case PolyType.Rectangle:
                        this.ptype = PolyType.Pentagon;
                        this.GetComponent<MeshFilter>().mesh = PolyTool.CreatePentagon();
                        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().mesh;
                        break;

                    case PolyType.Triangle:
                        this.ptype = PolyType.Rectangle;
                        this.GetComponent<MeshFilter>().mesh = PolyTool.CreateSquare();
                        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().mesh;
                        break;
                }
                break;
            case MorphType.Projectiles:
                ProjectileLauncher pl = GetComponent<ProjectileLauncher>();
                if (pl != null) {
                    pl.Upgrade();
                }
                else {
                    this.gameObject.AddComponent<ProjectileLauncher>();
                }
                break;
            case MorphType.Rotation:
                Rotator rot = GetComponent<Rotator>();
                if (rot != null) {
                    rot.Upgrade();
                }
                else {
                    this.gameObject.AddComponent<Rotator>();
                }
                break;
            case MorphType.Movement:
                Mover m = GetComponent<Mover>();
                if (m != null) {
                    m.Upgrade();
                }
                else {
                    this.gameObject.AddComponent<Mover>();
                }
                break;
        }
        timer.Reset();
    }

}
