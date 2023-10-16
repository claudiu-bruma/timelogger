import { NewProject } from "../models/NewProject";
import { Project } from "../models/Project";

const BASE_URL = "http://localhost:3001/api";

export  const getAll= async (): Promise<Project[]> => {
    const response = await fetch(`${BASE_URL}/projects`);
    const data = await response.json(); 
    return data as Project[];
}
// add another function for getting a signle project
export  const getProjectById= async (id :number): Promise<Project[]> => {
    const response = await fetch(`${BASE_URL}/projects/${id}`);
    const data = await response.json();
    return data as Project[];
}
// add a fucntion to create a new project   
export async function createProject(project : NewProject) {
    const response = await fetch(`${BASE_URL}/projects`, {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(project),
    }); 
    return response.ok;
}


