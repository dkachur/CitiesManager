export class City {
  id: string | null;
  name: string | null;

  constructor(id: string | null = null, name: string | null = null) {
    this.id = id;
    this.name = name;
  }
}
