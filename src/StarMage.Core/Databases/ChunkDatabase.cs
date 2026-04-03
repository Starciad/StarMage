using StarMage.Core.Chunks;
using StarMage.Core.Enums;

using System;

namespace StarMage.Core.Databases
{
    internal static class ChunkDatabase
    {
        private static readonly Chunk[] chunks =
        [

            #region STARTING CHUNKS

            new(ChunkType.Beginning, new TileType[,]
            {
                { TileType.T12, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.C03, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.T20, TileType.T15, TileType.T16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN },
                { TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN },
                { TileType.T13, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.T13, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.T18, TileType.T10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.T03, TileType.NNN, TileType.NNN },
                { TileType.T12, TileType.T13, TileType.NNN, TileType.B01, TileType.B02, TileType.B02, TileType.B03, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B07, TileType.NNN, TileType.T03, TileType.T03, TileType.T03, TileType.NNN },
                { TileType.T12, TileType.T18, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T10, TileType.NNN },
                { TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.NNN },
            }),

            #endregion

            #region NORMAL CHUNKS

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.C01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.B04, TileType.B05, TileType.B05, TileType.B06, TileType.NNN, TileType.E01, TileType.C02, TileType.E01, TileType.NNN, TileType.B07, TileType.NNN, TileType.E01, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T10, TileType.NNN },
                { TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.NNN },
            }),

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.C01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.B14, TileType.B15, TileType.B16, TileType.NNN },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E01, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.E01, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E04, TileType.T08, TileType.T09, TileType.T10, TileType.E03, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.B04, TileType.B06, TileType.C02, TileType.T11, TileType.T12, TileType.T13, TileType.C02, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.T08, TileType.T09, TileType.T17, TileType.T12, TileType.T18, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T17, TileType.T12, TileType.T18, TileType.T09, TileType.T10, TileType.NNN },
                { TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.NNN },
            }),

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.C01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.B08, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.E01, TileType.B07, TileType.E01 },
                { TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T10 },
                { TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.E01, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.C02, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T10, TileType.T04, TileType.T04, TileType.T04, TileType.T04, TileType.T04, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.NNN, TileType.NNN, TileType.B07, TileType.E01, TileType.NNN, TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.NNN, TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T17, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B04, TileType.B06, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.T03, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T17, TileType.T12, TileType.T13 },
                { TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T18, TileType.T09, TileType.T10, TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T13 },
            }),

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.E01, TileType.T03, TileType.E01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E01, TileType.B07, TileType.NNN, TileType.E01, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T10, TileType.T04, TileType.T04, TileType.T04, TileType.T08, TileType.T09, TileType.T09, TileType.T10, TileType.NNN },
                { TileType.E01, TileType.B01, TileType.B02, TileType.B03, TileType.E01, TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.C01, TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T13, TileType.E03 },
                { TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T17, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T14, TileType.T15, TileType.T15, TileType.T16, TileType.NNN },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.T03, TileType.C02, TileType.E01, TileType.B04, TileType.B05, TileType.B05, TileType.B06, TileType.E01 },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.T04, TileType.T04, TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T10 },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13 },
            }),

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.C01, TileType.NNN },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B01, TileType.B02, TileType.B02, TileType.B03, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T21, TileType.T22, TileType.T22, TileType.T22, TileType.T22, TileType.T23, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.C02, TileType.NNN, TileType.NNN, TileType.T24, TileType.T25, TileType.T25, TileType.T25, TileType.T25, TileType.T26, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.T21, TileType.T22, TileType.T23, TileType.NNN, TileType.T24, TileType.T25, TileType.T25, TileType.T25, TileType.T25, TileType.T26, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.T24, TileType.T25, TileType.T26, TileType.NNN, TileType.T24, TileType.T25, TileType.T25, TileType.T25, TileType.T25, TileType.T26, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.E01, TileType.NNN, TileType.T27, TileType.T28, TileType.T29, TileType.E01, TileType.T27, TileType.T28, TileType.T28, TileType.T28, TileType.T28, TileType.T29, TileType.E01, TileType.B04, TileType.B06, TileType.NNN },
                { TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T10 },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13 },
            }),

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.C01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E01, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.T04, TileType.T04, TileType.T04, TileType.T08, TileType.T09, TileType.T10, TileType.T04, TileType.T04, TileType.T04, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E01, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.B01, TileType.B02, TileType.B03, TileType.E01, TileType.T03, TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T03, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.T08, TileType.T09, TileType.T10, TileType.T03, TileType.T03, TileType.T11, TileType.T12, TileType.T13, TileType.T03, TileType.B04, TileType.B06, TileType.T03, TileType.T03, TileType.T03, TileType.C02, TileType.E01 },
                { TileType.T11, TileType.T12, TileType.T18, TileType.T09, TileType.T09, TileType.T17, TileType.T12, TileType.T18, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T10 },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13 },
            }),

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.C01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B01, TileType.B03, TileType.C02, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN },
                { TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08, TileType.T08, TileType.T09, TileType.T09, TileType.T10, TileType.B08, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN },
                { TileType.E01, TileType.B07, TileType.NNN, TileType.NNN, TileType.E01, TileType.B07, TileType.T11, TileType.T12, TileType.T12, TileType.T13, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.B07, TileType.E01 },
                { TileType.T08, TileType.T10, TileType.T04, TileType.T04, TileType.T08, TileType.T09, TileType.T17, TileType.T12, TileType.T12, TileType.T18, TileType.T09, TileType.T10, TileType.T04, TileType.T04, TileType.T08, TileType.T10 },
                { TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T13 },
                { TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T13 },
            }),
            
            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.B09, TileType.NNN },
                { TileType.E05, TileType.NNN, TileType.T08, TileType.T10, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN },
                { TileType.NNN, TileType.E04, TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B04, TileType.B06, TileType.E01, TileType.C02, TileType.B07, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.T11, TileType.T13, TileType.T03, TileType.E01, TileType.B01, TileType.B03, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T10 },
                { TileType.NNN, TileType.E04, TileType.T11, TileType.T18, TileType.T09, TileType.T09, TileType.T09, TileType.T10, TileType.T04, TileType.T04, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13 },
                { TileType.NNN, TileType.NNN, TileType.T14, TileType.T15, TileType.T15, TileType.T15, TileType.T15, TileType.T16, TileType.NNN, TileType.NNN, TileType.T14, TileType.T15, TileType.T15, TileType.T19, TileType.T12, TileType.T13 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E02, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E02, TileType.C01, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.E01, TileType.B04, TileType.B05, TileType.B05, TileType.B06, TileType.NNN, TileType.E01, TileType.NNN, TileType.T03, TileType.T03, TileType.NNN, TileType.E01, TileType.NNN, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T17, TileType.T12, TileType.T13 },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13 },
            }),
            
            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN },
                { TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B04, TileType.B06, TileType.E01, TileType.C02, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E01, TileType.B07 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E04, TileType.T11, TileType.T12, TileType.T20, TileType.T15, TileType.T15, TileType.T16, TileType.NNN, TileType.NNN, TileType.T08, TileType.T09, TileType.T10 },
                { TileType.B01, TileType.B02, TileType.B03, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.C01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.T08, TileType.T09, TileType.T10, TileType.T04, TileType.T04, TileType.T11, TileType.T12, TileType.T13, TileType.E01, TileType.NNN, TileType.E01, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T18, TileType.T09, TileType.T09, TileType.T10, TileType.T04, TileType.T04, TileType.T11, TileType.T12, TileType.T13 },
                { TileType.T11, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T13 },
            }),

            new(ChunkType.Normal, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.C01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.E05, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.B07, TileType.E01, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B08, TileType.NNN },
                { TileType.NNN, TileType.B04, TileType.B06, TileType.NNN, TileType.NNN, TileType.T08, TileType.T10, TileType.NNN, TileType.NNN, TileType.T08, TileType.T10, TileType.NNN, TileType.NNN, TileType.C02, TileType.B07, TileType.NNN },
                { TileType.NNN, TileType.T08, TileType.T10, TileType.NNN, TileType.NNN, TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.T08, TileType.T10, TileType.NNN },
                { TileType.NNN, TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T13, TileType.NNN, TileType.NNN, TileType.T11, TileType.T13, TileType.NNN },
            }),

            #endregion

            #region ENDING CHUNKS

            new(ChunkType.Ending, new TileType[,]
            {
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12, TileType.T12, TileType.T12 },
                { TileType.NNN, TileType.B11, TileType.B12, TileType.B13, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T14, TileType.T15, TileType.T19, TileType.T12 },
                { TileType.NNN, TileType.B14, TileType.B15, TileType.B16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.C04, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B09, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11, TileType.T12 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B10, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T03, TileType.T11, TileType.T12 },
                { TileType.NNN, TileType.B01, TileType.B02, TileType.B02, TileType.B03, TileType.NNN, TileType.NNN, TileType.NNN, TileType.B07, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T03, TileType.T03, TileType.T11, TileType.T12 },
                { TileType.T08, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T17, TileType.T12 },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12 },
            }),

            #endregion

            #region ARENA CHUNKS

            new(ChunkType.Arena, new TileType[,]
            {
                { TileType.T11, TileType.T20, TileType.T16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T14, TileType.T19, TileType.T12, TileType.T16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T14, TileType.T19, TileType.T12 },
                { TileType.T14, TileType.T16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T14, TileType.T16, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T14, TileType.T19 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.E06, TileType.NNN, TileType.T11 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T11 },
                { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.T21, TileType.T22, TileType.T23, TileType.NNN, TileType.NNN, TileType.T21, TileType.T22, TileType.T23, TileType.NNN, TileType.B09, TileType.NNN, TileType.T11 },
                { TileType.NNN, TileType.T03, TileType.B08, TileType.NNN, TileType.T24, TileType.T25, TileType.T26, TileType.NNN, TileType.NNN, TileType.T24, TileType.T25, TileType.T26, TileType.NNN, TileType.B10, TileType.NNN, TileType.T11 },
                { TileType.T08, TileType.T10, TileType.B07, TileType.NNN, TileType.T27, TileType.T28, TileType.T29, TileType.NNN, TileType.NNN, TileType.T27, TileType.T28, TileType.T29, TileType.NNN, TileType.B07, TileType.T08, TileType.T17 },
                { TileType.T11, TileType.T18, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T09, TileType.T17, TileType.T12 },
                { TileType.T11, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12, TileType.T12 },
            }),

            #endregion

        ];

        internal static Chunk GetRandomChunk(Predicate<Chunk> predicate)
        {
            Chunk selected = null;
            int count = 0;

            for (int i = 0; i < chunks.Length; i++)
            {
                Chunk chunk = chunks[i];

                if (!predicate(chunk))
                {
                    continue;
                }

                count++;

                if (Randomness.Random.Range(1, count - 1) == 1)
                {
                    selected = chunk;
                }
            }

            return selected ?? throw new InvalidOperationException("No chunks match the given predicate.");
        }
    }
}

/*

[TEMPLATE]

new(ChunkType.None, new TileType[,]
{
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
    { TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN, TileType.NNN },
}),

[CLOUND]

TileType.B11, TileType.B12, TileType.B13
TileType.B14, TileType.B15, TileType.B16

*/