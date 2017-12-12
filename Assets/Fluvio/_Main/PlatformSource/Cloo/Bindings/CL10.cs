#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID
using System;
using System.Runtime.InteropServices;

namespace Cloo.Bindings
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    public class CL10 : ICL10
    {
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        protected const string libName = "OpenCL.dll";
        #elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        protected const string libName = "/System/Library/Frameworks/OpenCL.framework/Versions/Current/OpenCL";
        #elif UNITY_STANDALONE_LINUX || UNITY_ANDROID
        protected const string libName = "OpenCL";
        #endif

        #region External
        [DllImport(libName, EntryPoint = "clGetPlatformIDs")]
        extern static ComputeErrorCode GetPlatformIDs(
            Int32 num_entries,
            [Out, MarshalAs(UnmanagedType.LPArray)] CLPlatformHandle[] platforms,
            out Int32 num_platforms);
        [DllImport(libName, EntryPoint = "clGetPlatformInfo")]
        extern static ComputeErrorCode GetPlatformInfo(
            CLPlatformHandle platform,
            ComputePlatformInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clGetDeviceIDs")]
        extern static ComputeErrorCode GetDeviceIDs(
            CLPlatformHandle platform,
            ComputeDeviceTypes device_type,
            Int32 num_entries,
            [Out, MarshalAs(UnmanagedType.LPArray)] CLDeviceHandle[] devices,
            out Int32 num_devices);
        [DllImport(libName, EntryPoint = "clGetDeviceInfo")]
        extern static ComputeErrorCode GetDeviceInfo(
            CLDeviceHandle device,
            ComputeDeviceInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clCreateContext")]
        extern static CLContextHandle CreateContext(
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] properties,
            Int32 num_devices,
            [MarshalAs(UnmanagedType.LPArray)] CLDeviceHandle[] devices,
            ComputeContextNotifier pfn_notify,
            IntPtr user_data,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateContextFromType")]
        extern static CLContextHandle CreateContextFromType(
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] properties,
            ComputeDeviceTypes device_type,
            ComputeContextNotifier pfn_notify,
            IntPtr user_data,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clRetainContext")]
        extern static ComputeErrorCode RetainContext(
            CLContextHandle context);
        [DllImport(libName, EntryPoint = "clReleaseContext")]
        extern static ComputeErrorCode ReleaseContext(
            CLContextHandle context);
        [DllImport(libName, EntryPoint = "clGetContextInfo")]
        extern static ComputeErrorCode GetContextInfo(
            CLContextHandle context,
            ComputeContextInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clCreateCommandQueue")]
        extern static CLCommandQueueHandle CreateCommandQueue(
            CLContextHandle context,
            CLDeviceHandle device,
            ComputeCommandQueueFlags properties,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clRetainCommandQueue")]
        extern static ComputeErrorCode RetainCommandQueue(
            CLCommandQueueHandle command_queue);
        [DllImport(libName, EntryPoint = "clReleaseCommandQueue")]
        extern static ComputeErrorCode
        ReleaseCommandQueue(
            CLCommandQueueHandle command_queue);
        [DllImport(libName, EntryPoint = "clGetCommandQueueInfo")]
        extern static ComputeErrorCode GetCommandQueueInfo(
            CLCommandQueueHandle command_queue,
            ComputeCommandQueueInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clSetCommandQueueProperty")]
        extern static ComputeErrorCode SetCommandQueueProperty(
            CLCommandQueueHandle command_queue,
            ComputeCommandQueueFlags properties,
            [MarshalAs(UnmanagedType.Bool)] bool enable,
            out ComputeCommandQueueFlags old_properties);
        [DllImport(libName, EntryPoint = "clCreateBuffer")]
        extern static CLMemoryHandle CreateBuffer(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            IntPtr size,
            IntPtr host_ptr,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateImage2D")]
        extern static CLMemoryHandle CreateImage2D(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            ref ComputeImageFormat image_format,
            IntPtr image_width,
            IntPtr image_height,
            IntPtr image_row_pitch,
            IntPtr host_ptr,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateImage3D")]
        extern static CLMemoryHandle CreateImage3D(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            ref ComputeImageFormat image_format,
            IntPtr image_width,
            IntPtr image_height,
            IntPtr image_depth,
            IntPtr image_row_pitch,
            IntPtr image_slice_pitch,
            IntPtr host_ptr,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clRetainMemObject")]
        extern static ComputeErrorCode RetainMemObject(
            CLMemoryHandle memobj);
        [DllImport(libName, EntryPoint = "clReleaseMemObject")]
        extern static ComputeErrorCode ReleaseMemObject(
            CLMemoryHandle memobj);
        [DllImport(libName, EntryPoint = "clGetSupportedImageFormats")]
        extern static ComputeErrorCode GetSupportedImageFormats(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            ComputeMemoryType image_type,
            Int32 num_entries,
            [Out, MarshalAs(UnmanagedType.LPArray)] ComputeImageFormat[] image_formats,
            out Int32 num_image_formats);
        [DllImport(libName, EntryPoint = "clGetMemObjectInfo")]
        extern static ComputeErrorCode GetMemObjectInfo(
            CLMemoryHandle memobj,
            ComputeMemoryInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clGetImageInfo")]
        extern static ComputeErrorCode GetImageInfo(
            CLMemoryHandle image,
            ComputeImageInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clCreateSampler")]
        extern static CLSamplerHandle CreateSampler(
            CLContextHandle context,
            [MarshalAs(UnmanagedType.Bool)] bool normalized_coords,
            ComputeImageAddressing addressing_mode,
            ComputeImageFiltering filter_mode,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clRetainSampler")]
        extern static ComputeErrorCode RetainSampler(
            CLSamplerHandle sample);
        [DllImport(libName, EntryPoint = "clReleaseSampler")]
        extern static ComputeErrorCode ReleaseSampler(
            CLSamplerHandle sample);
        [DllImport(libName, EntryPoint = "clGetSamplerInfo")]
        extern static ComputeErrorCode GetSamplerInfo(
            CLSamplerHandle sample,
            ComputeSamplerInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clCreateProgramWithSource")]
        extern static CLProgramHandle CreateProgramWithSource(
            CLContextHandle context,
            Int32 count,
            String[] strings,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] lengths,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateProgramWithBinary")]
        extern static CLProgramHandle CreateProgramWithBinary(
            CLContextHandle context,
            Int32 num_devices,
            [MarshalAs(UnmanagedType.LPArray)] CLDeviceHandle[] device_list,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] lengths,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] binaries,
            [MarshalAs(UnmanagedType.LPArray)] Int32[] binary_status,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clRetainProgram")]
        extern static ComputeErrorCode RetainProgram(
            CLProgramHandle program);
        [DllImport(libName, EntryPoint = "clReleaseProgram")]
        extern static ComputeErrorCode ReleaseProgram(
            CLProgramHandle program);
        [DllImport(libName, EntryPoint = "clBuildProgram")]
        extern static ComputeErrorCode BuildProgram(
            CLProgramHandle program,
            Int32 num_devices,
            [MarshalAs(UnmanagedType.LPArray)] CLDeviceHandle[] device_list,
            String options,
            ComputeProgramBuildNotifier pfn_notify,
            IntPtr user_data);
        [DllImport(libName, EntryPoint = "clUnloadCompiler")]
        extern static ComputeErrorCode UnloadCompiler();
        [DllImport(libName, EntryPoint = "clGetProgramInfo")]
        extern static ComputeErrorCode GetProgramInfo(
            CLProgramHandle program,
            ComputeProgramInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clGetProgramBuildInfo")]
        extern static ComputeErrorCode GetProgramBuildInfo(
            CLProgramHandle program,
            CLDeviceHandle device,
            ComputeProgramBuildInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clCreateKernel")]
        extern static CLKernelHandle CreateKernel(
            CLProgramHandle program,
            String kernel_name,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateKernelsInProgram")]
        extern static ComputeErrorCode CreateKernelsInProgram(
            CLProgramHandle program,
            Int32 num_kernels,
            [Out, MarshalAs(UnmanagedType.LPArray)] CLKernelHandle[] kernels,
            out Int32 num_kernels_ret);
        [DllImport(libName, EntryPoint = "clRetainKernel")]
        extern static ComputeErrorCode RetainKernel(
            CLKernelHandle kernel);
        [DllImport(libName, EntryPoint = "clReleaseKernel")]
        extern static ComputeErrorCode ReleaseKernel(
            CLKernelHandle kernel);
        [DllImport(libName, EntryPoint = "clSetKernelArg")]
        extern static ComputeErrorCode SetKernelArg(
            CLKernelHandle kernel,
            Int32 arg_index,
            IntPtr arg_size,
            IntPtr arg_value);
        [DllImport(libName, EntryPoint = "clGetKernelInfo")]
        extern static ComputeErrorCode GetKernelInfo(
            CLKernelHandle kernel,
            ComputeKernelInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clGetKernelWorkGroupInfo")]
        extern static ComputeErrorCode GetKernelWorkGroupInfo(
            CLKernelHandle kernel,
            CLDeviceHandle device,
            ComputeKernelWorkGroupInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clWaitForEvents")]
        extern static ComputeErrorCode WaitForEvents(
            Int32 num_events,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_list);
        [DllImport(libName, EntryPoint = "clGetEventInfo")]
        extern static ComputeErrorCode GetEventInfo(
            CLEventHandle @event,
            ComputeEventInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clRetainEvent")]
        extern static ComputeErrorCode RetainEvent(
            CLEventHandle @event);
        [DllImport(libName, EntryPoint = "clReleaseEvent")]
        extern static ComputeErrorCode ReleaseEvent(
            CLEventHandle @event);
        [DllImport(libName, EntryPoint = "clGetEventProfilingInfo")]
        extern static ComputeErrorCode GetEventProfilingInfo(
            CLEventHandle @event,
            ComputeCommandProfilingInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clFlush")]
        extern static ComputeErrorCode Flush(
            CLCommandQueueHandle command_queue);
        [DllImport(libName, EntryPoint = "clFinish")]
        extern static ComputeErrorCode Finish(
            CLCommandQueueHandle command_queue);
        [DllImport(libName, EntryPoint = "clEnqueueReadBuffer")]
        extern static ComputeErrorCode EnqueueReadBuffer(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle buffer,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_read,
            IntPtr offset,
            IntPtr cb,
            IntPtr ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueWriteBuffer")]
        extern static ComputeErrorCode EnqueueWriteBuffer(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle buffer,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_write,
            IntPtr offset,
            IntPtr cb,
            IntPtr ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueCopyBuffer")]
        extern static ComputeErrorCode EnqueueCopyBuffer(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle src_buffer,
            CLMemoryHandle dst_buffer,
            IntPtr src_offset,
            IntPtr dst_offset,
            IntPtr cb,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueReadImage")]
        extern static ComputeErrorCode EnqueueReadImage(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle image,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_read,
            ref SysIntX3 origin,
            ref SysIntX3 region,
            IntPtr row_pitch,
            IntPtr slice_pitch,
            IntPtr ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueWriteImage")]
        extern static ComputeErrorCode EnqueueWriteImage(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle image,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_write,
            ref SysIntX3 origin,
            ref SysIntX3 region,
            IntPtr input_row_pitch,
            IntPtr input_slice_pitch,
            IntPtr ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueCopyImage")]
        extern static ComputeErrorCode EnqueueCopyImage(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle src_image,
            CLMemoryHandle dst_image,
            ref SysIntX3 src_origin,
            ref SysIntX3 dst_origin,
            ref SysIntX3 region,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueCopyImageToBuffer")]
        extern static ComputeErrorCode EnqueueCopyImageToBuffer(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle src_image,
            CLMemoryHandle dst_buffer,
            ref SysIntX3 src_origin,
            ref SysIntX3 region,
            IntPtr dst_offset,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueCopyBufferToImage")]
        extern static ComputeErrorCode EnqueueCopyBufferToImage(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle src_buffer,
            CLMemoryHandle dst_image,
            IntPtr src_offset,
            ref SysIntX3 dst_origin,
            ref SysIntX3 region,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueMapBuffer")]
        extern static IntPtr EnqueueMapBuffer(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle buffer,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_map,
            ComputeMemoryMappingFlags map_flags,
            IntPtr offset,
            IntPtr cb,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clEnqueueMapImage")]
        extern static IntPtr EnqueueMapImage(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle image,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_map,
            ComputeMemoryMappingFlags map_flags,
            ref SysIntX3 origin,
            ref SysIntX3 region,
            out IntPtr image_row_pitch,
            out IntPtr image_slice_pitch,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clEnqueueUnmapMemObject")]
        extern static ComputeErrorCode EnqueueUnmapMemObject(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle memobj,
            IntPtr mapped_ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueNDRangeKernel")]
        extern static ComputeErrorCode EnqueueNDRangeKernel(
            CLCommandQueueHandle command_queue,
            CLKernelHandle kernel,
            Int32 work_dim,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] global_work_offset,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] global_work_size,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] local_work_size,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueTask")]
        extern static ComputeErrorCode EnqueueTask(
            CLCommandQueueHandle command_queue,
            CLKernelHandle kernel,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        /*
        [DllImport(libName, EntryPoint = "clEnqueueNativeKernel")]
        extern static ComputeErrorCode EnqueueNativeKernel(
            CLCommandQueueHandle command_queue,
            IntPtr user_func,
            IntPtr args,
            IntPtr cb_args,
            Int32 num_mem_objects,
            IntPtr* mem_list,
            IntPtr* args_mem_loc,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst=1)] CLEventHandle[] new_event);
        */
        [DllImport(libName, EntryPoint = "clEnqueueMarker")]
        extern static ComputeErrorCode EnqueueMarker(
            CLCommandQueueHandle command_queue,
            out CLEventHandle new_event);
        [DllImport(libName, EntryPoint = "clEnqueueWaitForEvents")]
        extern static ComputeErrorCode EnqueueWaitForEvents(
            CLCommandQueueHandle command_queue,
            Int32 num_events,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_list);
        [DllImport(libName, EntryPoint = "clEnqueueBarrier")]
        extern static ComputeErrorCode EnqueueBarrier(
            CLCommandQueueHandle command_queue);
        [DllImport(libName, EntryPoint = "clGetExtensionFunctionAddress")]
        extern static IntPtr GetExtensionFunctionAddress(
            String func_name);
        [DllImport(libName, EntryPoint = "clCreateFromGLBuffer")]
        extern static CLMemoryHandle CreateFromGLBuffer(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            Int32 bufobj,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateFromGLTexture2D")]
        extern static CLMemoryHandle CreateFromGLTexture2D(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            Int32 target,
            Int32 miplevel,
            Int32 texture,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateFromGLTexture3D")]
        extern static CLMemoryHandle CreateFromGLTexture3D(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            Int32 target,
            Int32 miplevel,
            Int32 texture,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clCreateFromGLRenderbuffer")]
        extern static CLMemoryHandle CreateFromGLRenderbuffer(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            Int32 renderbuffer,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clGetGLObjectInfo")]
        extern static ComputeErrorCode GetGLObjectInfo(
            CLMemoryHandle memobj,
            out ComputeGLObjectType gl_object_type,
            out Int32 gl_object_name);
        [DllImport(libName, EntryPoint = "clGetGLTextureInfo")]
        extern static ComputeErrorCode GetGLTextureInfo(
            CLMemoryHandle memobj,
            ComputeGLTextureInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);
        [DllImport(libName, EntryPoint = "clEnqueueAcquireGLObjects")]
        extern static ComputeErrorCode EnqueueAcquireGLObjects(
            CLCommandQueueHandle command_queue,
            Int32 num_objects,
            [MarshalAs(UnmanagedType.LPArray)] CLMemoryHandle[] mem_objects,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueReleaseGLObjects")]
        extern static ComputeErrorCode EnqueueReleaseGLObjects(
            CLCommandQueueHandle command_queue,
            Int32 num_objects,
            [MarshalAs(UnmanagedType.LPArray)] CLMemoryHandle[] mem_objects,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        #endregion

        #region ICL10 implementation
        ComputeErrorCode ICL10.GetPlatformInfo(CLPlatformHandle platform, ComputePlatformInfo param_name, IntPtr param_value_size,
                                               IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetPlatformInfo(platform, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.GetDeviceIDs(CLPlatformHandle platform, ComputeDeviceTypes device_type, int num_entries,
                                            CLDeviceHandle[] devices, out int num_devices)
        {
            return GetDeviceIDs(platform, device_type, num_entries, devices, out num_devices);
        }
        ComputeErrorCode ICL10.GetDeviceInfo(CLDeviceHandle device, ComputeDeviceInfo param_name, IntPtr param_value_size,
                                             IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetDeviceInfo(device, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        CLContextHandle ICL10.CreateContext(IntPtr[] properties, int num_devices, CLDeviceHandle[] devices,
                                            ComputeContextNotifier pfn_notify, IntPtr user_data, out ComputeErrorCode errcode_ret)
        {
            return CreateContext(properties, num_devices, devices, pfn_notify, user_data, out errcode_ret);
        }
        CLContextHandle ICL10.CreateContextFromType(IntPtr[] properties, ComputeDeviceTypes device_type,
                                                    ComputeContextNotifier pfn_notify, IntPtr user_data,
                                                    out ComputeErrorCode errcode_ret)
        {
            return CreateContextFromType(properties, device_type, pfn_notify, user_data, out errcode_ret);
        }
        ComputeErrorCode ICL10.RetainContext(CLContextHandle context)
        {
            return RetainContext(context);
        }
        ComputeErrorCode ICL10.ReleaseContext(CLContextHandle context)
        {
            return ReleaseContext(context);
        }
        ComputeErrorCode ICL10.GetContextInfo(CLContextHandle context, ComputeContextInfo param_name, IntPtr param_value_size,
                                              IntPtr param_value, out IntPtr param_value_size_ret)
        {
            param_value_size_ret = IntPtr.Zero;
            return GetContextInfo(context, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        CLCommandQueueHandle ICL10.CreateCommandQueue(CLContextHandle context, CLDeviceHandle device,
                                                      ComputeCommandQueueFlags properties, out ComputeErrorCode errcode_ret)
        {
            return CreateCommandQueue(context, device, properties, out errcode_ret);
        }
        ComputeErrorCode ICL10.RetainCommandQueue(CLCommandQueueHandle command_queue)
        {
            return RetainCommandQueue(command_queue);
        }
        ComputeErrorCode ICL10.ReleaseCommandQueue(CLCommandQueueHandle command_queue)
        {
            return ReleaseCommandQueue(command_queue);
        }
        ComputeErrorCode ICL10.GetCommandQueueInfo(CLCommandQueueHandle command_queue, ComputeCommandQueueInfo param_name,
                                                   IntPtr param_value_size, IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetCommandQueueInfo(command_queue, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.SetCommandQueueProperty(CLCommandQueueHandle command_queue, ComputeCommandQueueFlags properties,
                                                       bool enable, out ComputeCommandQueueFlags old_properties)
        {
            return SetCommandQueueProperty(command_queue, properties, enable, out old_properties);
        }
        CLMemoryHandle ICL10.CreateBuffer(CLContextHandle context, ComputeMemoryFlags flags, IntPtr size, IntPtr host_ptr,
                                          out ComputeErrorCode errcode_ret)
        {
            return CreateBuffer(context, flags, size, host_ptr, out errcode_ret);
        }
        CLMemoryHandle ICL10.CreateImage2D(CLContextHandle context, ComputeMemoryFlags flags, ref ComputeImageFormat image_format,
                                           IntPtr image_width, IntPtr image_height, IntPtr image_row_pitch, IntPtr host_ptr,
                                           out ComputeErrorCode errcode_ret)
        {
            return CreateImage2D(context, flags, ref image_format, image_width, image_height, image_row_pitch, host_ptr, out errcode_ret);
        }
        CLMemoryHandle ICL10.CreateImage3D(CLContextHandle context, ComputeMemoryFlags flags, ref ComputeImageFormat image_format,
                                           IntPtr image_width, IntPtr image_height, IntPtr image_depth, IntPtr image_row_pitch,
                                           IntPtr image_slice_pitch, IntPtr host_ptr, out ComputeErrorCode errcode_ret)
        {
            return CreateImage3D(context, flags, ref image_format, image_width, image_height, image_depth, image_row_pitch, image_slice_pitch, host_ptr, out errcode_ret);
        }
        ComputeErrorCode ICL10.RetainMemObject(CLMemoryHandle memobj)
        {
            return RetainMemObject(memobj);
        }
        ComputeErrorCode ICL10.ReleaseMemObject(CLMemoryHandle memobj)
        {
            return ReleaseMemObject(memobj);
        }
        ComputeErrorCode ICL10.GetSupportedImageFormats(CLContextHandle context, ComputeMemoryFlags flags,
                                                        ComputeMemoryType image_type, int num_entries,
                                                        ComputeImageFormat[] image_formats, out int num_image_formats)
        {
            return GetSupportedImageFormats(context, flags, image_type, num_entries, image_formats, out num_image_formats);
        }
        ComputeErrorCode ICL10.GetMemObjectInfo(CLMemoryHandle memobj, ComputeMemoryInfo param_name, IntPtr param_value_size,
                                                IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetMemObjectInfo(memobj, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.GetImageInfo(CLMemoryHandle image, ComputeImageInfo param_name, IntPtr param_value_size,
                                            IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetImageInfo(image, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        CLSamplerHandle ICL10.CreateSampler(CLContextHandle context, bool normalized_coords, ComputeImageAddressing addressing_mode,
                                            ComputeImageFiltering filter_mode, out ComputeErrorCode errcode_ret)
        {
            return CreateSampler(context, normalized_coords, addressing_mode, filter_mode, out errcode_ret);
        }
        ComputeErrorCode ICL10.RetainSampler(CLSamplerHandle sample)
        {
            return RetainSampler(sample);
        }
        ComputeErrorCode ICL10.ReleaseSampler(CLSamplerHandle sample)
        {
            return ReleaseSampler(sample);
        }
        ComputeErrorCode ICL10.GetSamplerInfo(CLSamplerHandle sample, ComputeSamplerInfo param_name, IntPtr param_value_size,
                                              IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetSamplerInfo(sample, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        CLProgramHandle ICL10.CreateProgramWithSource(CLContextHandle context, int count, string[] strings, IntPtr[] lengths,
                                                      out ComputeErrorCode errcode_ret)
        {
            return CreateProgramWithSource(context, count, strings, lengths, out errcode_ret);
        }
        CLProgramHandle ICL10.CreateProgramWithBinary(CLContextHandle context, int num_devices, CLDeviceHandle[] device_list,
                                                      IntPtr[] lengths, IntPtr[] binaries, int[] binary_status,
                                                      out ComputeErrorCode errcode_ret)
        {
            return CreateProgramWithBinary(context, num_devices, device_list, lengths, binaries, binary_status, out errcode_ret);
        }
        ComputeErrorCode ICL10.RetainProgram(CLProgramHandle program)
        {
            return RetainProgram(program);
        }
        ComputeErrorCode ICL10.ReleaseProgram(CLProgramHandle program)
        {
            return ReleaseProgram(program);
        }
        ComputeErrorCode ICL10.BuildProgram(CLProgramHandle program, int num_devices, CLDeviceHandle[] device_list, string options,
                                            ComputeProgramBuildNotifier pfn_notify, IntPtr user_data)
        {
            return BuildProgram(program, num_devices, device_list, options, pfn_notify, user_data);
        }
        ComputeErrorCode ICL10.UnloadCompiler()
        {
            return UnloadCompiler();
        }
        ComputeErrorCode ICL10.GetProgramInfo(CLProgramHandle program, ComputeProgramInfo param_name, IntPtr param_value_size,
                                              IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetProgramInfo(program, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.GetProgramBuildInfo(CLProgramHandle program, CLDeviceHandle device, ComputeProgramBuildInfo param_name,
                                                   IntPtr param_value_size, IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetProgramBuildInfo(program, device, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        CLKernelHandle ICL10.CreateKernel(CLProgramHandle program, string kernel_name, out ComputeErrorCode errcode_ret)
        {
            return CreateKernel(program, kernel_name, out errcode_ret);
        }
        ComputeErrorCode ICL10.CreateKernelsInProgram(CLProgramHandle program, int num_kernels, CLKernelHandle[] kernels,
                                                      out int num_kernels_ret)
        {
            return CreateKernelsInProgram(program, num_kernels, kernels, out num_kernels_ret);
        }
        ComputeErrorCode ICL10.RetainKernel(CLKernelHandle kernel)
        {
            return RetainKernel(kernel);
        }
        ComputeErrorCode ICL10.ReleaseKernel(CLKernelHandle kernel)
        {
            return ReleaseKernel(kernel);
        }
        ComputeErrorCode ICL10.SetKernelArg(CLKernelHandle kernel, int arg_index, IntPtr arg_size, IntPtr arg_value)
        {
            return SetKernelArg(kernel, arg_index, arg_size, arg_value);
        }
        ComputeErrorCode ICL10.GetKernelInfo(CLKernelHandle kernel, ComputeKernelInfo param_name, IntPtr param_value_size,
                                             IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetKernelInfo(kernel, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.GetKernelWorkGroupInfo(CLKernelHandle kernel, CLDeviceHandle device,
                                                      ComputeKernelWorkGroupInfo param_name, IntPtr param_value_size,
                                                      IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetKernelWorkGroupInfo(kernel, device, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.WaitForEvents(int num_events, CLEventHandle[] event_list)
        {
            return WaitForEvents(num_events, event_list);
        }
        ComputeErrorCode ICL10.GetEventInfo(CLEventHandle @event, ComputeEventInfo param_name, IntPtr param_value_size,
                                            IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetEventInfo(@event, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.RetainEvent(CLEventHandle @event)
        {
            return RetainEvent(@event);
        }
        ComputeErrorCode ICL10.ReleaseEvent(CLEventHandle @event)
        {
            return ReleaseEvent(@event);
        }
        ComputeErrorCode ICL10.GetEventProfilingInfo(CLEventHandle @event, ComputeCommandProfilingInfo param_name,
                                                     IntPtr param_value_size, IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetEventProfilingInfo(@event, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.Flush(CLCommandQueueHandle command_queue)
        {
            return Flush(command_queue);
        }
        ComputeErrorCode ICL10.Finish(CLCommandQueueHandle command_queue)
        {
            return Finish(command_queue);
        }
        ComputeErrorCode ICL10.EnqueueReadBuffer(CLCommandQueueHandle command_queue, CLMemoryHandle buffer, bool blocking_read,
                                                 IntPtr offset, IntPtr cb, IntPtr ptr, int num_events_in_wait_list,
                                                 CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueReadBuffer(command_queue, buffer, blocking_read, offset, cb, ptr, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueWriteBuffer(CLCommandQueueHandle command_queue, CLMemoryHandle buffer, bool blocking_write,
                                                  IntPtr offset, IntPtr cb, IntPtr ptr, int num_events_in_wait_list,
                                                  CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueWriteBuffer(command_queue, buffer, blocking_write, offset, cb, ptr, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueCopyBuffer(CLCommandQueueHandle command_queue, CLMemoryHandle src_buffer,
                                                 CLMemoryHandle dst_buffer, IntPtr src_offset, IntPtr dst_offset, IntPtr cb,
                                                 int num_events_in_wait_list, CLEventHandle[] event_wait_list,
                                                 CLEventHandle[] new_event)
        {
            return EnqueueCopyBuffer(command_queue, src_buffer, dst_buffer, src_offset, dst_offset, cb, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueReadImage(CLCommandQueueHandle command_queue, CLMemoryHandle image, bool blocking_read,
                                                ref SysIntX3 origin, ref SysIntX3 region, IntPtr row_pitch, IntPtr slice_pitch,
                                                IntPtr ptr, int num_events_in_wait_list, CLEventHandle[] event_wait_list,
                                                CLEventHandle[] new_event)
        {
            return EnqueueReadImage(command_queue, image, blocking_read, ref origin, ref region, row_pitch, slice_pitch, ptr, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueWriteImage(CLCommandQueueHandle command_queue, CLMemoryHandle image, bool blocking_write,
                                                 ref SysIntX3 origin, ref SysIntX3 region, IntPtr input_row_pitch,
                                                 IntPtr input_slice_pitch, IntPtr ptr, int num_events_in_wait_list,
                                                 CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueWriteImage(command_queue, image, blocking_write, ref origin, ref region, input_row_pitch, input_slice_pitch, ptr, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueCopyImage(CLCommandQueueHandle command_queue, CLMemoryHandle src_image, CLMemoryHandle dst_image,
                                                ref SysIntX3 src_origin, ref SysIntX3 dst_origin, ref SysIntX3 region,
                                                int num_events_in_wait_list, CLEventHandle[] event_wait_list,
                                                CLEventHandle[] new_event)
        {
            return EnqueueCopyImage(command_queue, src_image, dst_image, ref src_origin, ref dst_origin, ref region, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueCopyImageToBuffer(CLCommandQueueHandle command_queue, CLMemoryHandle src_image,
                                                        CLMemoryHandle dst_buffer, ref SysIntX3 src_origin, ref SysIntX3 region,
                                                        IntPtr dst_offset, int num_events_in_wait_list,
                                                        CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueCopyImageToBuffer(command_queue, src_image, dst_buffer, ref src_origin, ref region, dst_offset, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueCopyBufferToImage(CLCommandQueueHandle command_queue, CLMemoryHandle src_buffer,
                                                        CLMemoryHandle dst_image, IntPtr src_offset, ref SysIntX3 dst_origin,
                                                        ref SysIntX3 region, int num_events_in_wait_list,
                                                        CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueCopyBufferToImage(command_queue, src_buffer, dst_image, src_offset, ref dst_origin, ref region, num_events_in_wait_list, event_wait_list, new_event);
        }
        IntPtr ICL10.EnqueueMapBuffer(CLCommandQueueHandle command_queue, CLMemoryHandle buffer, bool blocking_map,
                                      ComputeMemoryMappingFlags map_flags, IntPtr offset, IntPtr cb, int num_events_in_wait_list,
                                      CLEventHandle[] event_wait_list, CLEventHandle[] new_event, out ComputeErrorCode errcode_ret)
        {
            return EnqueueMapBuffer(command_queue, buffer, blocking_map, map_flags, offset, cb, num_events_in_wait_list, event_wait_list, new_event, out errcode_ret);
        }
        IntPtr ICL10.EnqueueMapImage(CLCommandQueueHandle command_queue, CLMemoryHandle image, bool blocking_map,
                                     ComputeMemoryMappingFlags map_flags, ref SysIntX3 origin, ref SysIntX3 region,
                                     out IntPtr image_row_pitch, out IntPtr image_slice_pitch, int num_events_in_wait_list,
                                     CLEventHandle[] event_wait_list, CLEventHandle[] new_event, out ComputeErrorCode errcode_ret)
        {
            return EnqueueMapImage(command_queue, image, blocking_map, map_flags, ref origin, ref region, out image_row_pitch, out image_slice_pitch, num_events_in_wait_list, event_wait_list, new_event, out errcode_ret);
        }
        ComputeErrorCode ICL10.EnqueueUnmapMemObject(CLCommandQueueHandle command_queue, CLMemoryHandle memobj, IntPtr mapped_ptr,
                                                     int num_events_in_wait_list, CLEventHandle[] event_wait_list,
                                                     CLEventHandle[] new_event)
        {
            return EnqueueUnmapMemObject(command_queue, memobj, mapped_ptr, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueNDRangeKernel(CLCommandQueueHandle command_queue, CLKernelHandle kernel, int work_dim,
                                                    IntPtr[] global_work_offset, IntPtr[] global_work_size, IntPtr[] local_work_size,
                                                    int num_events_in_wait_list, CLEventHandle[] event_wait_list,
                                                    CLEventHandle[] new_event)
        {
            return EnqueueNDRangeKernel(command_queue, kernel, work_dim, global_work_offset, global_work_size, local_work_size, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueTask(CLCommandQueueHandle command_queue, CLKernelHandle kernel, int num_events_in_wait_list,
                                           CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueTask(command_queue, kernel, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueMarker(CLCommandQueueHandle command_queue, out CLEventHandle new_event)
        {
            return EnqueueMarker(command_queue, out new_event);
        }
        ComputeErrorCode ICL10.EnqueueWaitForEvents(CLCommandQueueHandle command_queue, int num_events, CLEventHandle[] event_list)
        {
            return EnqueueWaitForEvents(command_queue, num_events, event_list);
        }
        ComputeErrorCode ICL10.EnqueueBarrier(CLCommandQueueHandle command_queue)
        {
            return EnqueueBarrier(command_queue);
        }
        IntPtr ICL10.GetExtensionFunctionAddress(string func_name)
        {
            return GetExtensionFunctionAddress(func_name);
        }
        CLMemoryHandle ICL10.CreateFromGLBuffer(CLContextHandle context, ComputeMemoryFlags flags, int bufobj,
                                                out ComputeErrorCode errcode_ret)
        {
            return CreateFromGLBuffer(context, flags, bufobj, out errcode_ret);
        }
        CLMemoryHandle ICL10.CreateFromGLTexture2D(CLContextHandle context, ComputeMemoryFlags flags, int target, int miplevel,
                                                   int texture, out ComputeErrorCode errcode_ret)
        {
            return CreateFromGLTexture2D(context, flags, target, miplevel, texture, out errcode_ret);
        }
        CLMemoryHandle ICL10.CreateFromGLTexture3D(CLContextHandle context, ComputeMemoryFlags flags, int target, int miplevel,
                                                   int texture, out ComputeErrorCode errcode_ret)
        {
            return CreateFromGLTexture3D(context, flags, target, miplevel, texture, out errcode_ret);
        }
        CLMemoryHandle ICL10.CreateFromGLRenderbuffer(CLContextHandle context, ComputeMemoryFlags flags, int renderbuffer,
                                                      out ComputeErrorCode errcode_ret)
        {
            return CreateFromGLRenderbuffer(context, flags, renderbuffer, out errcode_ret);
        }
        ComputeErrorCode ICL10.GetGLObjectInfo(CLMemoryHandle memobj, out ComputeGLObjectType gl_object_type, out int gl_object_name)
        {
            return GetGLObjectInfo(memobj, out gl_object_type, out gl_object_name);
        }
        ComputeErrorCode ICL10.GetGLTextureInfo(CLMemoryHandle memobj, ComputeGLTextureInfo param_name, IntPtr param_value_size,
                                                IntPtr param_value, out IntPtr param_value_size_ret)
        {
            return GetGLTextureInfo(memobj, param_name, param_value_size, param_value, out param_value_size_ret);
        }
        ComputeErrorCode ICL10.EnqueueAcquireGLObjects(CLCommandQueueHandle command_queue, int num_objects,
                                                       CLMemoryHandle[] mem_objects, int num_events_in_wait_list,
                                                       CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueAcquireGLObjects(command_queue, num_objects, mem_objects, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.EnqueueReleaseGLObjects(CLCommandQueueHandle command_queue, int num_objects,
                                                       CLMemoryHandle[] mem_objects, int num_events_in_wait_list,
                                                       CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueReleaseGLObjects(command_queue, num_objects, mem_objects, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL10.GetPlatformIDs(int num_entries, CLPlatformHandle[] platforms, out int num_platforms)
        {
            return GetPlatformIDs(num_entries, platforms, out num_platforms);
        }
#endregion
    }
}
#endif
