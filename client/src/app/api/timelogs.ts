import { NewTimeLog } from "../models/NewTimeLog";
import { TimeLog } from "../models/TimeLog";
import { CreateTimeLogResponse }  from "../models/CreateTimeLogResponse";

const BASE_URL = "http://localhost:3001/api"; 
export  const getTimelogsByProjectId= async (id :number | null): Promise<TimeLog[]> => {
    const response = await fetch(`${BASE_URL}/Timelogs?projectId=${id}`);
    const data = await response.json();
    return data as TimeLog[];
}

// add a fucntion to create a new timelog   
export async function createTimeLog(project : NewTimeLog) : Promise<CreateTimeLogResponse> {
    const response = await fetch(`${BASE_URL}/Timelogs`, {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(project),
    });
    const responseCode = response.status;  
    const body = await response.text();
 
    const apiRespones  : CreateTimeLogResponse ={
        statusCode : responseCode,
        message: body
    };
    console.log(apiRespones);
    return  apiRespones ;
}
