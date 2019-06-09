import { UserRole } from 'src/enums';

export class User {
    id: string;
    name: string;
    lastName: string;
    username: string;
    email: string;
    role: UserRole;
    password: string;

    constructor(
        name: string,
        lastName: string,
        username: string,
        email: string,
        role: UserRole,
        password: string) {
            this.name = name;
            this.lastName = lastName;
            this.username = username;
            this.email = email;
            this.password = password;
            this.role = role;
        }
}
