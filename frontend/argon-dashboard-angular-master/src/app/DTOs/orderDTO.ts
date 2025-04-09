import { WatercoolerDTO } from "./watercoolerDTO";

export class OrderDTO {
    porudzbinaID: string;
    dostavljena: boolean;
    datumPorudzbine: string;
    iznos: number;
    zaposleniID: string;
    klijentID: string;
    klijent: string;
    zaposleni: string;
    aparati: WatercoolerDTO[] = [];
}