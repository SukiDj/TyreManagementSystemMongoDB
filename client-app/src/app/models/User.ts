export interface User{
    ime: string;
    prezime:string;
    telefon:string;
    datumRodjenja:Date;
    token:string;
    role : string;
    userName:string;
}

export interface RegisterUserFormValues {
    email:string;
    password: string;
    ime: string;
    prezime: string;
    username: string;
    telefon: string;
    datumRodjenja: Date;
    role: string;
  }

export interface UserFormValues {
    email:string;
    password: string;
}