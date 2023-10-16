import React, { useState } from 'react';
import { createProject } from "../api/projects";  
import { NewProject } from "../models/NewProject";


export default function ProjectForm({ onClose }: AddProjectProps) {
    const [name, setName] = useState<string>('');
    const [deadline, setDeadline] = useState<Date | null>(null);
    const [isCompleted, setIsCompleted] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (name && deadline) {
            const newProject: NewProject = { name, deadline, isCompleted };
            console.log(newProject);
            const isResponseOk =  await createProject(newProject);
            console.log(isResponseOk);
            // Reset form
            setName('');
            setDeadline(null);
            setIsCompleted(false);
            onClose();
        }
    };

    return (
         
        <form id="addProjectForm" onSubmit={handleSubmit} className="space-y-4">
        <div className="flex flex-col">
            <label htmlFor="projectName" className="mb-2 font-medium">Project Name:</label>
            <input 
                id="projectName"
                value={name} 
                onChange={e => setName(e.target.value)} 
                className="p-2 border rounded-md"
            />
        </div>
    
        <div className="flex flex-col">
            <label htmlFor="deadline" className="mb-2 font-medium">Deadline:</label>
            <input 
                id="deadline"
                type="date" 
                onChange={e => setDeadline(new Date(e.target.value))} 
                className="p-2 border rounded-md"
            />
        </div>
    
        <div className="flex items-center space-x-2">
           <label htmlFor="completed" className="font-medium">Completed:</label> 
           <input 
                id="completed"
                type="checkbox" 
                checked={isCompleted} 
                onChange={e => setIsCompleted(e.target.checked)} 
                className="mr-2"
            />
            
        </div>
    
        <div className="flex space-x-2">       
            <button 
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"  
                type="submit"
            >
                Save
            </button>        
            <button 
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" 
                onClick={onClose}
            >
                Close
            </button>     
        </div>
    </form>
    );
}

interface AddProjectProps {
    onClose: () => void;
}