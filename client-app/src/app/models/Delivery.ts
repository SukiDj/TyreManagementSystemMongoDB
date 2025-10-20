export interface Delivery {
  id: string;
  client: string;
  origin: { type: 'Point'; coordinates: number[] };
  destination: { type: 'Point'; coordinates: number[] };
  distanceMeters: number;
  status: 'Pendidng' | 'Delivered';
  createdAt: string;
}
