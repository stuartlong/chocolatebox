using UnityEngine;
using System.Collections;

/**
 * Represents the various entrances/exits in a SectionBuilder.
 * A negative entrancePosition represents that that entrace does not exist
 * Stuart Long
 * */
public class EntrancePositions {
	public int westEntrance;
	public int eastEntrance;
	public int southEntrance;
	public int northEntrance;

	public EntrancePositions(int west, int east, int south, int north)
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
