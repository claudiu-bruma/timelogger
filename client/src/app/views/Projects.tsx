import React, { useState, useEffect } from 'react';
// import Table from "../components/Table";
import {getAll,Project} from "../api/projects";  
import Timelogs from './TimeLogs';
import AddProject from './AddProject';

export default function Projects() {
    const [projects, setProjects] = useState<Project[]>([]);
    const [selectedProjectId, setSelectedProjectId] = useState<number | null>(null);
    const [showProjectModal, setShowProjectModal] = useState<boolean>(false);
    const closeModal = async  (projectId : number)=>{
            setSelectedProjectId(projectId);
            fetchProjects();
        }
   const closeAddProjectModal = async  ()=>{
         setShowProjectModal(false);
          fetchProjects();
   }

    const fetchProjects = async () => {
        const result = await getAll();
        setProjects(result);
    }

    useEffect(() => { 
        fetchProjects();
    }, []); 

    if (selectedProjectId) {
        return (
            <div className="flex items-center my-6">
                <div className="w-1/2"> 
                    <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={() => setSelectedProjectId(null)}>Back</button>
                </div>                
                <Timelogs projectId={selectedProjectId} />
            </div>
        );
    }

    return (
        <>
            <div className="flex items-center my-6">
                <div className="w-1/2">
                    <button onClick={() => setShowProjectModal(true)} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                        Add entry
                    </button>
                </div>
                {showProjectModal && (
                    <AddProject onClose={() => closeAddProjectModal()} />
                )}
                <div className="w-1/2 flex justify-end">
                    <form>
                        <input
                            className="border rounded-full py-2 px-4"
                            type="search"
                            placeholder="Search"
                            aria-label="Search"
                        />
                        <button
                            className="bg-blue-500 hover:bg-blue-700 text-white rounded-full py-2 px-4 ml-2"
                            type="submit"
                        >
                            Search
                        </button>
                    </form>
                </div>
            </div>


            <table className="table-fixed w-full">
            <thead className="bg-gray-200">
                <tr>
                    <th className="border px-4 py-2 w-12">Id</th>
                    <th className="border px-4 py-2">Project Name</th>
                    <th className="border px-4 py-2">Deadline</th>
                    <th className="border px-4 py-2">Is Project Completed</th>
                    <th className="border px-4 py-2">Action</th>
                </tr>
            </thead>
            <tbody>


            {projects.map(project => (
                 <tr key = {project.id}>
                    <td className="border px-4 py-2 w-12">{project.id}</td>
                    <td className="border px-4 py-2">{project.name}</td>
                    <td className="border px-4 py-2">{project.deadline}</td>                    
                    <td className="border px-4 py-2"><input type="checkbox" disabled  checked={project.isCompleted}></input></td>
                    <td className="border px-4 py-2">
                        <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={() => closeModal(project.id)}>
                            View Time Logs
                        </button>   
                    </td>  
                 </tr>                    
                ))} 
            </tbody>
        </table> 
        {/* <Timelogs projectId={selectedProjectId} />
        <AddProject/> */}
        </>
    );
}
