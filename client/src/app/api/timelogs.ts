const BASE_URL = "http://localhost:3001/api";
// add another function for getting a signle project
export  const getTimelogsByProjectId= async (id :number | null): Promise<TimeLog[]> => {
    const response = await fetch(`${BASE_URL}/Timelogs?projectId=${id}`);
    const data = await response.json();
    return data as TimeLog[];
}

// add a fucntion to create a new timelog   
export async function createTimeLog(project : NewTimeLog) {
    const response = await fetch(`${BASE_URL}/Timelogs`, {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(project),
    });
    console.log(response);
    return response.ok;
}



export interface NewTimeLog {
    comment: string;
    logDate: Date;
    logTimeInMinutes: number;
    projectId: number;
}

export interface TimeLog {
    id: number;
    comment: string;
    logDate: Date;
    logTimeInMinutes: number;
    projectId: number;
}
