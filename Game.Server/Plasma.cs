using Source.Common;
using Source;
using Game.Shared;
using System.Numerics;
namespace Game.Server;
using FIELD = FIELD<Plasma>;
[LinkEntityToClass("_plasma")]
[NetworkName("CPlasma")]
public class Plasma : BaseFire
{
	public const int bitsFIRESMOKE_SMOKE = 0x00000002; // this probably shouldnt be redefined here...
	public const int bitsPLASMA_FREE = 0x00000002;

	public static readonly SendTable DT_Plasma = new(DT_BaseEntity, [
		SendPropFloat(FIELD.OF(nameof(flScale)), 0, PropFlags.NoScale),
		SendPropFloat(FIELD.OF(nameof(ScaleTime)), 0, PropFlags.NoScale),
		SendPropInt(FIELD.OF(nameof(Flags)), 8, PropFlags.Unsigned),
		SendPropInt(FIELD.OF(nameof(PlasmaModelIndex)), 14, 0),
		SendPropInt(FIELD.OF(nameof(PlasmaModelIndex2)), 14, 0),
		SendPropInt(FIELD.OF(nameof(GlowModelIndex)), 14, 0),
	]);
	public static readonly new ServerClass ServerClass = new ServerClass(DT_Plasma);

	[NetworkName("m_nPlasmaModelIndex")]
	public int PlasmaModelIndex;
	[NetworkName("m_nPlasmaModelIndex2")]
	public int PlasmaModelIndex2;
	[NetworkName("m_nGlowModelIndex")]
	public int GlowModelIndex;

	public Plasma() {
		flScale = 0.0f;
		ScaleTime = 0.0f;
		Flags = bitsFIRE_NONE;
		PlasmaModelIndex = PrecacheModel("sprites/plasma1.vmt");
		PlasmaModelIndex2 = PrecacheModel("sprites/plasma1.vmt");
		GlowModelIndex = PrecacheModel("sprites/fire_floor.vmt");
	}

	//-----------------------------------------------------------------------------
	// Purpose: 
	// Input  : state - 
	//-----------------------------------------------------------------------------
	public virtual void EnableSmoke(int state) {
		if (state != 0)
			Flags |= bitsFIRESMOKE_SMOKE;
		else
			Flags &= ~bitsFIRESMOKE_SMOKE;
	}

	public override void Precache() {
		PrecacheModel("sprites/plasma1.vmt");
		PrecacheModel("sprites/fire_floor.vmt");
	}
}
