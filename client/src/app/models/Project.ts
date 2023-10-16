//define a new typescipt object project with id, name, description, and completed

export interface Project {
    id: number;
    name: string;
    deadline: Date;
    isCompleted: boolean;
}
