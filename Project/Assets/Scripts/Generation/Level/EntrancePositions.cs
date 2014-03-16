using UnityEngine;
using System.Collections;

/**
 * Represents the various entrances/exits in a SectionBuilder.
 * A negative entrancePosition represents that that entrace does not exist
 * Stuart Long
 * */
public class EntrancePositions {
	public EntrancePosition westEntrance;
	public EntrancePosition eastEntrance;
	public EntrancePosition southEntrance;
	public EntrancePosition northEntrance;

	public EntrancePositions(EntrancePosition west, EntrancePosition east, EntrancePosition south, EntrancePosition north)
	{
		westEntrance = west;
		eastEntrance = east;
		southEntrance = south;
		northEntrance = north;
	}

	public EntrancePositions(EntrancePositions copy)
	{
		westEntrance = copy.westEntrance;
		eastEntrance = copy.eastEntrance;
		southEntrance = copy.southEntrance;
		northEntrance = copy.northEntrance;
	}
}
