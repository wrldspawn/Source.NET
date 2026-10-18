using Source.Common;
using Source;
using Game.Shared;
using System.Numerics;
namespace Game.Server;

using FIELD_FS = FIELD<FireSmoke>;

[NetworkName("CBaseFire")]
public class BaseFire : BaseEntity
{
	public const int bitsFIRE_NONE = 0x00000000;
	public const int bitsFIRE_ACTIVE = 0x00000001;

	public static readonly new DataMap DataDesc = new(typeof(BaseFire), BaseEntity.DataDesc, [
		DEFINE<BaseFire>.FIELD(nameof(StartScale), FieldType.Float),
		DEFINE<BaseFire>.FIELD(nameof(flScale), FieldType.Float),
		DEFINE<BaseFire>.FIELD(nameof(ScaleTime), FieldType.Float),
		DEFINE<BaseFire>.FIELD(nameof(Flags), FieldType.Integer),
	]);
	public override DataMap? GetDataDescMap() => DataDesc;

	[NetworkName("m_flStartScale")]
	public float StartScale;
	[NetworkName("m_flScale")]
	public float flScale;
	[NetworkName("m_flScaleTime")]
	public float ScaleTime;
	[NetworkName("m_nFlags")]
	public int Flags;

	public BaseFire() {
		StartScale = 0.0f;
		flScale = 0.0f;
		ScaleTime = 0.0f;
		Flags = bitsFIRE_NONE;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Take the current scale of the flame and move it towards a destination
	// Input  : size - destination size
	//			time - time to scale across
	//-----------------------------------------------------------------------------
	public virtual void Scale(float size, float time) {
		flScale = size;
		ScaleTime = time;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Overloaded Scale() function to set size
	// Input  : start - beginning sizek
	//			size - destination size
	//			time - time to scale across
	//-----------------------------------------------------------------------------
	public virtual void Scale(float start, float size, float time) {
		StartScale = start;
		flScale = size;
		ScaleTime = time;
	}

	//-----------------------------------------------------------------------------
	// Purpose: 
	// Input  : state - 
	//-----------------------------------------------------------------------------
	public virtual void Enable(int state) {
		if (state != 0)
			Flags |= bitsFIRE_ACTIVE;
		else
			Flags &= ~bitsFIRE_ACTIVE;
	}
}

[LinkEntityToClass("_firesmoke")]
[NetworkName("CFireSmoke")]
public class FireSmoke : BaseFire
{
	public const int bitsFIRESMOKE_SMOKE = 0x00000002;
	public const int bitsFIRESMOKE_SMOKE_COLLISION = 0x00000004;
	public const int bitsFIRESMOKE_GLOW = 0x00000008;
	public const int bitsFIRESMOKE_VISIBLE_FROM_ABOVE = 0x00000010;

	public static readonly SendTable DT_FireSmoke = new(DT_BaseEntity, [
		SendPropFloat(FIELD_FS.OF(nameof(StartScale)), 0, PropFlags.NoScale),
		SendPropFloat(FIELD_FS.OF(nameof(flScale)), 0, PropFlags.NoScale),
		SendPropFloat(FIELD_FS.OF(nameof(ScaleTime)), 0, PropFlags.NoScale),
		SendPropInt(FIELD_FS.OF(nameof(Flags)), 8, PropFlags.Unsigned),
		SendPropInt(FIELD_FS.OF(nameof(FlameModelIndex)), 14, 0),
		SendPropInt(FIELD_FS.OF(nameof(FlameFromAboveModelIndex)), 14, 0),
	]);
	public static readonly new ServerClass ServerClass = new ServerClass(DT_FireSmoke);

	public static readonly new DataMap DataDesc = new(typeof(FireSmoke), BaseFire.DataDesc, [
		DEFINE<FireSmoke>.FIELD(nameof(FlameModelIndex), FieldType.ModelIndex),
		DEFINE<FireSmoke>.FIELD(nameof(FlameFromAboveModelIndex), FieldType.ModelIndex),
	]);
	public override DataMap? GetDataDescMap() => DataDesc;

	[NetworkName("m_nFlameModelIndex")]
	public int FlameModelIndex;
	[NetworkName("m_nFlameFromAboveModelIndex")]
	public int FlameFromAboveModelIndex;

	public FireSmoke() {
		flScale = 0.0f;
		ScaleTime = 0.0f;
		Flags = bitsFIRE_NONE;

		AddEFlags(EFL.ForceCheckTransmit);
	}

	public override void Spawn() {
		Precache();
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

	//-----------------------------------------------------------------------------
	// Purpose: 
	// Input  : state - 
	//-----------------------------------------------------------------------------
	public virtual void EnableGlow(int state) {
		if (state != 0)
			Flags |= bitsFIRESMOKE_GLOW;
		else
			Flags &= ~bitsFIRESMOKE_GLOW;
	}

	//-----------------------------------------------------------------------------
	// Purpose: 
	// Input  : state - 
	//-----------------------------------------------------------------------------
	public virtual void EnableVisibleFromAbove(int state) {
		if (state != 0)
			Flags |= bitsFIRESMOKE_VISIBLE_FROM_ABOVE;
		else
			Flags &= ~bitsFIRESMOKE_VISIBLE_FROM_ABOVE;
	}
}
