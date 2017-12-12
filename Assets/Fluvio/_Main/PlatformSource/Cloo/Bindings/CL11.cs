#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID
using System;
using System.Runtime.InteropServices;

namespace Cloo.Bindings
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    public class CL11 : CL10, ICL11
    {
        #region External
        [DllImport(libName, EntryPoint = "clCreateSubBuffer")]
        extern static CLMemoryHandle CreateSubBuffer(
            CLMemoryHandle buffer,
            ComputeMemoryFlags flags,
            ComputeBufferCreateType buffer_create_type,
            ref SysIntX2 buffer_create_info,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clSetMemObjectDestructorCallback")]
        extern static ComputeErrorCode SetMemObjectDestructorCallback(
            CLMemoryHandle memobj,
            ComputeMemoryDestructorNotifer pfn_notify,
            IntPtr user_data);
        [DllImport(libName, EntryPoint = "clCreateUserEvent")]
        extern static CLEventHandle CreateUserEvent(
            CLContextHandle context,
            out ComputeErrorCode errcode_ret);
        [DllImport(libName, EntryPoint = "clSetUserEventStatus")]
        extern static ComputeErrorCode SetUserEventStatus(
            CLEventHandle @event,
            Int32 execution_status);
        [DllImport(libName, EntryPoint = "clSetEventCallback")]
        extern static ComputeErrorCode SetEventCallback(
            CLEventHandle @event,
            Int32 command_exec_callback_type,
            ComputeEventCallback pfn_notify,
            IntPtr user_data);
        [DllImport(libName, EntryPoint = "clEnqueueReadBufferRect")]
        extern static ComputeErrorCode EnqueueReadBufferRect(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle buffer,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_read,
            ref SysIntX3 buffer_offset,
            ref SysIntX3 host_offset,
            ref SysIntX3 region,
            IntPtr buffer_row_pitch,
            IntPtr buffer_slice_pitch,
            IntPtr host_row_pitch,
            IntPtr host_slice_pitch,
            IntPtr ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueWriteBufferRect")]
        extern static ComputeErrorCode EnqueueWriteBufferRect(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle buffer,
            [MarshalAs(UnmanagedType.Bool)] bool blocking_write,
            ref SysIntX3 buffer_offset,
            ref SysIntX3 host_offset,
            ref SysIntX3 region,
            IntPtr buffer_row_pitch,
            IntPtr buffer_slice_pitch,
            IntPtr host_row_pitch,
            IntPtr host_slice_pitch,
            IntPtr ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        [DllImport(libName, EntryPoint = "clEnqueueCopyBufferRect")]
        extern static ComputeErrorCode EnqueueCopyBufferRect(
            CLCommandQueueHandle command_queue,
            CLMemoryHandle src_buffer,
            CLMemoryHandle dst_buffer,
            ref SysIntX3 src_origin,
            ref SysIntX3 dst_origin,
            ref SysIntX3 region,
            IntPtr src_row_pitch,
            IntPtr src_slice_pitch,
            IntPtr dst_row_pitch,
            IntPtr dst_slice_pitch,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);
        #endregion

        #region ICL11 implementation
        ComputeErrorCode ICL11.SetMemObjectDestructorCallback(CLMemoryHandle memobj, ComputeMemoryDestructorNotifer pfn_notify,
                                                              IntPtr user_data)
        {
            return SetMemObjectDestructorCallback(memobj, pfn_notify, user_data);
        }
        CLEventHandle ICL11.CreateUserEvent(CLContextHandle context, out ComputeErrorCode errcode_ret)
        {
            return CreateUserEvent(context, out errcode_ret);
        }
        ComputeErrorCode ICL11.SetUserEventStatus(CLEventHandle @event, int execution_status)
        {
            return SetUserEventStatus(@event, execution_status);
        }
        ComputeErrorCode ICL11.SetEventCallback(CLEventHandle @event, int command_exec_callback_type, ComputeEventCallback pfn_notify,
                                                IntPtr user_data)
        {
            return SetEventCallback(@event, command_exec_callback_type, pfn_notify, user_data);
        }
        ComputeErrorCode ICL11.EnqueueReadBufferRect(CLCommandQueueHandle command_queue, CLMemoryHandle buffer, bool blocking_read,
                                                     ref SysIntX3 buffer_offset, ref SysIntX3 host_offset, ref SysIntX3 region,
                                                     IntPtr buffer_row_pitch, IntPtr buffer_slice_pitch, IntPtr host_row_pitch,
                                                     IntPtr host_slice_pitch, IntPtr ptr, int num_events_in_wait_list,
                                                     CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueReadBufferRect(command_queue, buffer, blocking_read, ref buffer_offset, ref host_offset, ref region, buffer_row_pitch, buffer_slice_pitch, host_row_pitch, host_slice_pitch, ptr, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL11.EnqueueWriteBufferRect(CLCommandQueueHandle command_queue, CLMemoryHandle buffer, bool blocking_write,
                                                      ref SysIntX3 buffer_offset, ref SysIntX3 host_offset, ref SysIntX3 region,
                                                      IntPtr buffer_row_pitch, IntPtr buffer_slice_pitch, IntPtr host_row_pitch,
                                                      IntPtr host_slice_pitch, IntPtr ptr, int num_events_in_wait_list,
                                                      CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueWriteBufferRect(command_queue, buffer, blocking_write, ref buffer_offset, ref host_offset, ref region, buffer_row_pitch, buffer_slice_pitch, host_row_pitch, host_slice_pitch, ptr, num_events_in_wait_list, event_wait_list, new_event);
        }
        ComputeErrorCode ICL11.EnqueueCopyBufferRect(CLCommandQueueHandle command_queue, CLMemoryHandle src_buffer,
                                                     CLMemoryHandle dst_buffer, ref SysIntX3 src_origin, ref SysIntX3 dst_origin,
                                                     ref SysIntX3 region, IntPtr src_row_pitch, IntPtr src_slice_pitch,
                                                     IntPtr dst_row_pitch, IntPtr dst_slice_pitch, int num_events_in_wait_list,
                                                     CLEventHandle[] event_wait_list, CLEventHandle[] new_event)
        {
            return EnqueueCopyBufferRect(command_queue, src_buffer, dst_buffer, ref src_origin, ref dst_origin, ref region, src_row_pitch, src_slice_pitch, dst_row_pitch, dst_slice_pitch, num_events_in_wait_list, event_wait_list, new_event);
        }
        [Obsolete("Deprecated in OpenCL 1.1.")]        
        ComputeErrorCode ICL11.SetCommandQueueProperty(CLCommandQueueHandle command_queue, ComputeCommandQueueFlags properties,
                                                       bool enable, out ComputeCommandQueueFlags old_properties)
        {
            return ((ICL10)this).SetCommandQueueProperty(command_queue, properties, enable, out old_properties);
        }
        CLMemoryHandle ICL11.CreateSubBuffer(CLMemoryHandle buffer, ComputeMemoryFlags flags,
                                             ComputeBufferCreateType buffer_create_type, ref SysIntX2 buffer_create_info,
                                             out ComputeErrorCode errcode_ret)
        {
            return CreateSubBuffer(buffer, flags, buffer_create_type, ref buffer_create_info, out errcode_ret);
        }
        #endregion
    }
}
#endif
