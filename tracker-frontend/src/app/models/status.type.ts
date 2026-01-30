import { Coordinate } from './coordinate.type';

export type Status = {
  offset?: number;
  station?: number;
  closestPoint?: Coordinate;
  currentLineIndex?: number;
};
